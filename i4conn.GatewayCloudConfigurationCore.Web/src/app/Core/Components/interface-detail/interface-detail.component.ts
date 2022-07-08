import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { SelectTypeEntity } from '../../Models/Entity.model';
import { interfaceGroup } from '../../Models/InterfaceGroup.model';
import { Mode } from '../../Models/Mode.model';
import { EntityParam, ParamInterface } from '../../Models/ParamInterface.model';
import { InterfaceService } from '../../Services/interface.service';
import { InterfaceGroupService } from '../../Services/interfacegroups.service';
import { ParamInterfaceService } from '../../Services/paramInterface.service';
import { typeEntityService } from '../../Services/typeEntity.service';
import { InterfaceGroupsComponent } from '../interface-groups/interface-groups.component';
import { TypeEntityComponent } from '../type-entity/type-entity.component';
import { debounceTime, delay, filter, map, takeUntil, tap } from 'rxjs/operators';
import { ChannelService } from '../../Services/channel.service';
import { Channel } from '../../Models/Channel.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Interface } from '../../Models/Interface.model';
import { MatDialog } from '@angular/material/dialog';
import { GatewayService } from '../../Services/gateway.service';
import { Gateway } from '../../Models/Gateway.model';
import * as moment from 'moment';

@Component({
  selector: 'app-interface-detail',
  templateUrl: './interface-detail.component.html',
  styleUrls: ['./interface-detail.component.scss']
})
export class InterfaceDetailComponent implements OnInit, OnDestroy {
  subscribers: Subscription[];

  gatewayId = this.route.snapshot.paramMap.get('gatewayId');
  groupId = this.route.snapshot.paramMap.get('groupId');
  interfaceId = this.route.snapshot.paramMap.get('interfaceId');

  title = "Interfaccia " + this.interfaceId;

  hoverConfig = false;
  hoverBack = false;
  hoverSave = false;

  gateway: Gateway | undefined = undefined;
  interfaceGroup: interfaceGroup | undefined = undefined;
  interface: Interface | undefined = undefined;
  mode: Mode = Mode.Edit;

  parameters = false;
  channels = false;
  virtualChannels = false;

  selectTypeEntity: SelectTypeEntity[] = [];
  /** control for filter for server side. */
  filterTypeEntity: FormControl = new FormControl();
  /** indicate search operation is in progress */
  searching = false;
  /** list of typeEntity filtered after simulating server side search */
  filteredTypeEntity: ReplaySubject<SelectTypeEntity[]> = new ReplaySubject<SelectTypeEntity[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  interfaceGroupForm = this.formBuilder.group({
    id: ["", [Validators.required, Validators.maxLength(10)]],
    description: ["", [Validators.required, Validators.maxLength(100)]],
    gatewayId: this.gatewayId,
    interfaces: []
  });
  interfaceForm = this.formBuilder.group({
    interfaceId: "",
    interfaceDescription: ["", [Validators.required, Validators.maxLength(80)]],
    terminalIp: ["", [Validators.maxLength(30)]],
    terminalPort: ["", [Validators.maxLength(10)]],
    deviceId: "",
    router: ["", [Validators.maxLength(16)]],
    inputChannelNumber: "",
    outputChannelNumber: "",
    interfaceGroupId: ["", [Validators.required, Validators.maxLength(10)]],
    interfaceGroupDescription: ["", [Validators.required, Validators.maxLength(100)]],
    lastInterrogation: "",
    typologyInterface: ["", [Validators.required, Validators.maxLength(10)]],
    coordinator: ["", [Validators.maxLength(1)]],
  });

  channelsColumns: string[] = ['direction', 'description', 'actions'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private gatewayService: GatewayService,
    private interfaceGroupService: InterfaceGroupService,
    private interfaceService: InterfaceService,
    private typeEntityService: typeEntityService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  ngOnInit(): void {
    this.getGatewayDetails();
    this.getInterfaceGroup();
    //this.getInterface();
    this.getAllTypology();

    // filtra la lista in base al valore dell'input
    this.filterTypeEntity.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.searching = true),
        takeUntil(this._onDestroy),
        debounceTime(200),
        map(search => {
          if (!this.selectTypeEntity) {
            return [];
          }
          // simulate server fetching and filtering data
          return this.selectTypeEntity.filter(x => x.description.indexOf(search) > -1);
        }),
        delay(200),
        takeUntil(this._onDestroy)
      )
      .subscribe(filtered => {
        this.searching = false;
        this.filteredTypeEntity.next(filtered);
      },
        error => {
          this.searching = false;
        });

    if (this.interface != null) {
      this.getNChannel(this.interface.typologyInterface);
    }
  }

  show($event: any) {
    if ($event == 1) {
      this.parameters = true;
      this.channels = false;
      this.virtualChannels = false;
    }
    else if ($event == 2) {
      this.parameters = false;
      this.channels = true;
      this.virtualChannels = false;
    }
    else if ($event == 3) {
      this.parameters = false;
      this.channels = false;
      this.virtualChannels = true;
    }
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

  back() {
    this.router.navigate(['gateway/interfacegroups', this.gatewayId]);
  }

  getInterfaceGroup() {
    this.subscribers.push(this.interfaceGroupService.getById(this.groupId).subscribe(
      res => {
        this.interfaceGroup = res;
        this.getInterface();
      }
    ))
  }

  getInterface() {
    this.subscribers.push(this.interfaceService.getById(this.interfaceId).subscribe(
      res => {
        this.interface = res;
        this.interfaceForm = this.formBuilder.group({
          interfaceId: this.interface?.interfaceId,
          interfaceDescription: [this.interface?.interfaceDescription, [Validators.required, Validators.maxLength(80)]],
          terminalIp: [this.interface?.terminalIp, [Validators.maxLength(30)]],
          terminalPort: [this.interface?.terminalPort, [Validators.maxLength(10)]],
          deviceId: this.interface?.deviceId,
          router: [this.interface?.router, [Validators.maxLength(16)]],
          inputChannelNumber: this.interface?.inputChannelNumber,
          outputChannelNumber: this.interface?.outputChannelNumber,
          interfaceGroupId: [this.interfaceGroup?.id, [Validators.required, Validators.maxLength(10)]],
          interfaceGroupDescription: [this.interfaceGroup?.description, [Validators.required, Validators.maxLength(100)]],
          lastInterrogation: "",
          typologyInterface: [this.interfaceGroup?.interfaces[0].typologyInterface, [Validators.required, Validators.maxLength(10)]],
          coordinator: [this.interface?.coordinator, [Validators.maxLength(1)]],
        });
      }
    ));
  }

  getAllTypology() {
    this.selectTypeEntity = [];
    this.subscribers.push(this.typeEntityService.getByInterfaccia().subscribe(
      res => {
        for (var i = 0; i < res.length; i++)
        {
            var typeEntity =
            {
                id: res[i].id,
                description: res[i].id + " " + res[i].description
            };
            this.selectTypeEntity.push(typeEntity);
        }
        this.filteredTypeEntity.next(this.selectTypeEntity);
      }
    ));
  }

  addTypology() {
    let width;
    if (this.getWidth() < 400) { width = '90%'; }
    else { width = '60%'; }

    const dialogRef = this.dialog.open(TypeEntityComponent, {
      width: width,
      maxWidth: '90%',
      data: { mode: Mode.New, entity: "INTERFACCIA" }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); this.interfaceForm.get('typologyInterface')?.setValue(result.id); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); this.interfaceForm.get('typologyInterface')?.setValue(result.id); });
  }

  removeTypology() {
    this.interfaceForm.get('typologyInterface')?.setValue("");
  }

  addGroup() {}

  getNChannel(id: string) {
    if (id != "") {
      this.subscribers.push(this.typeEntityService.TypeInterfacesInputOutputNumber(id).subscribe(
        res => {
          this.interfaceForm.get('inputChannelNumber')?.setValue(res.inputNumber);
          this.interfaceForm.get('outputChannelNumber')?.setValue(res.outputNumber);

          if (res.inputNumber == 0 && res.outputNumber == 0) {
            this.interfaceForm.get('inputChannelNumber')?.setValue(16);
            this.interfaceForm.get('outputChannelNumber')?.setValue(16);
            this.interfaceForm.get('inputChannelNumber')?.enable();
            this.interfaceForm.get('outputChannelNumber')?.enable();
          }
          else {
            this.interfaceForm.get('inputChannelNumber')?.disable();
            this.interfaceForm.get('outputChannelNumber')?.disable();
          }
        }
      ));
    }
  }

  buttonDisabled() {
    if (this.IsNewMode() && this.interfaceForm.valid) {
      return false;
    }
    if (this.IsEditMode())
      return false;
    else
      return true;
  }

  onSubmit() {
    console.log(this.interfaceForm.value);
    this.interface = {
      interfaceId: this.interfaceForm.get('interfaceId')?.value,
      interfaceDescription: this.interfaceForm.get('interfaceDescription')?.value,
      terminalIp: this.interfaceForm.get('terminalIp')?.value,
      terminalPort: this.interfaceForm.get('terminalPort')?.value,
      deviceId: this.interfaceForm.get('deviceId')?.value,
      router: this.interfaceForm.get('router')?.value,
      inputChannelNumber: this.interfaceForm.get('inputChannelNumber')?.value,
      outputChannelNumber: this.interfaceForm.get('outputChannelNumber')?.value,
      interfaceGroupId: this.interfaceForm.get('interfaceGroupId')?.value,
      interfaceGroupDescription: this.interfaceForm.get('interfaceGroupDescription')?.value,
      lastInterrogation: "2021-03-18T14:40:36.750Z",
      typologyInterface: this.interfaceForm.get('typologyInterface')?.value,
      coordinator: this.interfaceForm.get('coordinator')?.value,
    }
    this.interfaceForm.get('lastInterrogation')?.setValue(moment(new Date()));
    this.subscribers.push(this.interfaceService.edit(this.interface).subscribe(
      res => {
        this.snackBar.open(res.message, "Done", {
          duration: 2000,
          panelClass: "success"
        });
      }
    ));
  }

  deleteInterface() {
    this.subscribers.push(this.interfaceService.delete(this.interface?.interfaceId).subscribe(
      res => {
        this.snackBar.open(res.message, "Done", {
          duration: 2000,
          panelClass: "Success"
        });
      }
    ))
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
