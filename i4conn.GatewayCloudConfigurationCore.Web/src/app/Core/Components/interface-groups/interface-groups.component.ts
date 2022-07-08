import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { debounceTime, delay, filter, map, takeUntil, tap } from 'rxjs/operators';
import Swal from 'sweetalert2';import { Gateway } from '../../Models/Gateway.model';
import { Interface } from '../../Models/Interface.model';
import { interfaceGroup } from '../../Models/InterfaceGroup.model';
import { Mode } from '../../Models/Mode.model';
import { GatewayService } from '../../Services/gateway.service';
import { InterfaceService } from '../../Services/interface.service';
import { InterfaceGroupService } from '../../Services/interfacegroups.service';
import { typeEntityService } from '../../Services/typeEntity.service';
import { InterfaceGroupDatailComponent } from './interface-groups-datail/interface-group-datail.component';
import { TypeEntityComponent } from '../type-entity/type-entity.component';

@Component({
  selector: 'app-interface-groups',
  templateUrl: './interface-groups.component.html',
  styleUrls: ['./interface-groups.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0', display: 'none'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class InterfaceGroupsComponent implements OnInit, OnDestroy {
  showInterfaces = true;
  title = "Gruppi interfacce";
  subscribers: Subscription[] = [];
  entity_INTERFACCIA = "INTERFACCIA";
  gatewayId = this.route.snapshot.paramMap.get('gatewayId');
  gateway: Gateway | undefined = undefined;
  hoverConf = false; hoverBack = false;
  adding = false;
  deleting = false;
  interfaceGroups: interfaceGroup[] = [];
  columnsToDisplay = ['id', 'description', 'typology', 'actions'];
  expandedElement: Interface[] = [];
  interfacesToAdding: Interface[] = [];

  /** control for filter for server side. */
  public filterInterfaces: FormControl = new FormControl();
  /** indicate search operation is in progress */
  public searching = false;
  /** list of typeEntity filtered after simulating server side search */
  public filteredInterfaces: ReplaySubject<Interface[]> = new ReplaySubject<Interface[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  interfaceForm = this.formBuilder.group({
    typologyInterface: "",
    interfaceDescription: ["", [Validators.required, Validators.maxLength(80)]],
    terminalIp: ["", [Validators.maxLength(30)]],
    terminalPort: ["", [Validators.maxLength(10)]],
    deviceId: 0,
    router: ["", [Validators.maxLength(16)]],
    coordinator: ["", [Validators.maxLength(1)]],
    inputChannelNumber: 0,
    outputChannelNumber: 0,
    interfaceGroupId: "",
    interfaceGroupDescription: ""
  });

  availableAdaptersForm = this.formBuilder.group({
    interfaceId: ["", [Validators.required]]
  })

  constructor(
    private route: ActivatedRoute,
    private service: InterfaceGroupService,
    private gatewayService: GatewayService,
    private interfaceService: InterfaceService,
    private typeEntityService: typeEntityService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.getGatewayDetails();
    this.getInterfaceGroups();

    // filtra la lista in base al valore dell'input
    this.filterInterfaces.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.searching = true),
        takeUntil(this._onDestroy),
        debounceTime(200),
        map(search => {
          if (!this.interfacesToAdding) {
            return [];
          }
          // simulate server fetching and filtering data
          return this.interfacesToAdding.filter(x => x.interfaceDescription.toUpperCase().indexOf(search) > -1);
        }),
        delay(200),
        takeUntil(this._onDestroy)
      )
      .subscribe(filtered => {
        this.searching = false;
        this.filteredInterfaces.next(filtered);
      },
        error => {
          this.searching = false;
        });
  }

  back() {
    this.router.navigate(['home']);
  }

  getGatewayDetails() {
    this.subscribers.push(this.gatewayService.getById(this.gatewayId).subscribe(
      res => {
        this.gateway = res;
      }
    ));
  }

  gatewayConfig() {
    this.router.navigate(['/gateway', this.gatewayId]);
  }

  getInterfaceGroups() {
    this.subscribers.push(this.service.getByGatewayId(this.gatewayId).subscribe(
      res => {
        this.interfaceGroups = res;
      }
    ));
  }

  getInterfacesToAdding(groupId: string) {
    this.interfacesToAdding = [];
    if (this.expandedElement.length == 0) {
      this.subscribers.push(this.interfaceService.getAvailableAdaptersByGroup(groupId).subscribe(
        res => {
          this.interfacesToAdding = res;
          this.filteredInterfaces.next(this.interfacesToAdding);
        }
      ));
    }
  }

  addInterfaceGroup() {
    const dialogRef = this.dialog.open(InterfaceGroupDatailComponent, {
      width: '90%',
      maxWidth: '90%',
      data: { mode: Mode.New, gatewayId: this.gatewayId, showForm: this.showInterfaces }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  editInterfaceGroup(interfaceGroup: interfaceGroup) {

    const dialogRef = this.dialog.open(InterfaceGroupDatailComponent, {
      width: '90%',
      maxWidth: '90%',
      data: { mode: Mode.Edit, gatewayId: this.gatewayId, interfaceGroup: interfaceGroup }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  deleteInterfaceGroup(id: string) {
    Swal.fire({
      title: 'Vuoi eliminare questo gruppo e tutte le interfacce associate?',
      icon: 'question',
      showDenyButton: true,
      showCancelButton: true,
      denyButtonText: "Elimina",
      confirmButtonText: 'Mantieni interfacce',
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subscribers.push(this.service.DeleteWithInterfaces(id).subscribe(
          res => {
            this.ngOnInit();
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
          }
        ));
      }
      if (result.isDenied) {
        this.subscribers.push(this.service.Delete(id).subscribe(
          res => {
            this.ngOnInit();
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
          }
        ));
      }
    });
  }

  Create() {
    this.adding = true;
    let typology = this.interfaceGroups[0].interfaces[0].typologyInterface;
    if (typology != "") {
      this.subscribers.push(this.typeEntityService.TypeInterfacesInputOutputNumber(typology).subscribe(
        res => {
          this.interfaceForm.get('inputChannelNumber')?.setValue(res.inputNumber);
          this.interfaceForm.get('outputChannelNumber')?.setValue(res.outputNumber);

          if (res.inputNumber == 0 && res.outputNumber == 0) {
            this.interfaceForm.get('inputChannelNumber')?.setValue(16);
            this.interfaceForm.get('outputChannelNumber')?.setValue(16);
          }
          else {
            this.interfaceForm.get('inputChannelNumber')?.disable();
            this.interfaceForm.get('outputChannelNumber')?.disable();
          }
        }
      ));
    }
  }

  addInterface(interfaceGroup: interfaceGroup) {
    this.interfaceForm.get('interfaceGroupId')?.setValue(interfaceGroup.id);
    this.interfaceForm.get('interfaceGroupDescription')?.setValue(interfaceGroup.description);
    this.interfaceForm.get('typologyInterface')?.setValue(interfaceGroup.interfaces[0].typologyInterface);
    this.subscribers.push(this.interfaceService.add(this.interfaceForm.value).subscribe(
      res => {
        this.snackBar.open(res.message, "Done", {
          duration: 2000,
          panelClass: "success"
        });
        this.ngOnInit();
        this.interfaceForm.reset();
      }
    ));
  }

  addInterfaceById(interfaceGroup: interfaceGroup) {
    var id = this.availableAdaptersForm.get('interfaceId')?.value;
    this.subscribers.push(this.interfaceService.getById(id).subscribe(
      res => {
        res.interfaceGroupId = interfaceGroup.id;
        res.interfaceGroupDescription = interfaceGroup.description;
        this.subscribers.push(this.interfaceService.edit(res).subscribe(
          ok => {
            this.snackBar.open(ok.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
            this.ngOnInit();
          }
        ));
      }
    ));
  }

  editInterface(interfaceGroup: interfaceGroup, data: Interface) {
    if (this.deleting == false)
      this.router.navigate(['/interface-detail/'+this.gatewayId+'/'+interfaceGroup.id+'/'+data.interfaceId]);
  }

  DeleteInterface(element: interfaceGroup,interfaceId: string) {
    this.deleting = true;
    if (element.interfaces.length > 1) {
      Swal.fire({
        title: 'Sicuro di voler eliminare questa interfaccia?',
        icon: 'question',
        showDenyButton: true,
        showCancelButton: true,
        showConfirmButton: false,
        denyButtonText: "Elimina",
        cancelButtonText: 'Annulla'
      }).then((result) => {
        if (result.isDenied) {
          this.subscribers.push(this.interfaceService.delete(interfaceId).subscribe(
            res => {
              this.deleting = false;
              this.subscribers.push(this.service.getByGatewayId(this.gatewayId).subscribe(
                end => {
                  this.interfaceGroups = end;
                  this.snackBar.open(res.message, "Done", {
                    duration: 2000,
                    panelClass: "success"
                  });
                }
              ));
            }
          ));
        }
        else {
          this.deleting = false;
        }
      });
    }
    else {
      Swal.fire({
        title: "Eliminando l'interfaccia eliminarai anche il gruppo di appartenenza. Eliminare?",
        icon: 'warning',
        showDenyButton: true,
        showCancelButton: true,
        showConfirmButton: false,
        denyButtonText: "Elimina",
        cancelButtonText: 'Annulla'
      }).then((result) => {
        if (result.isDenied) {
          this.subscribers.push(this.service.DeleteWithInterfaces(element.id).subscribe(
            res => {
              this.subscribers.push(this.service.getByGatewayId(this.gatewayId).subscribe(
                end => {
                  this.interfaceGroups = end;
                  this.snackBar.open(res.message, "Done", {
                    duration: 2000,
                    panelClass: "success"
                  });
                }
              ));
            }
          ));
        }
        else {
          this.deleting = false;
        }
      });
    }
  }

  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
