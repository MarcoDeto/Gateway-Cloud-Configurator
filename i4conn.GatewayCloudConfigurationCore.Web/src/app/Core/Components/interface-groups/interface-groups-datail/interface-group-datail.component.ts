import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as moment from 'moment';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { SelectTypeEntity } from '../../../Models/Entity.model';
import { Interface } from '../../../Models/Interface.model';
import { interfaceGroup } from '../../../Models/InterfaceGroup.model';
import { Mode } from '../../../Models/Mode.model';
import { EntityParam, ParamInterface } from '../../../Models/ParamInterface.model';
import { InterfaceService } from '../../../Services/interface.service';
import { InterfaceGroupService } from '../../../Services/interfacegroups.service';
import { ParamInterfaceService } from '../../../Services/paramInterface.service';
import { typeEntityService } from '../../../Services/typeEntity.service';
import { InterfaceGroupsComponent } from '../interface-groups.component';
import { TypeEntityComponent } from '../../type-entity/type-entity.component';
import { debounceTime, delay, filter, map, takeUntil, tap } from 'rxjs/operators';
import { ChannelService } from '../../../Services/channel.service';
import { Channel } from '../../../Models/Channel.model';

@Component({
  selector: 'app-interface-group-datail',
  templateUrl: './interface-group-datail.component.html',
  styleUrls: ['./interface-group-datail.component.scss']
})
export class InterfaceGroupDatailComponent implements OnInit, OnDestroy {
  subscribers: Subscription[];
  mode: Mode = Mode.New;
  interfaceGroup: interfaceGroup | undefined = undefined;
  interfaceId  = "";
  typologyInterface = "";
  parameters: EntityParam[] = [];
  channels: Channel[] = [];

  selectTypeEntity: SelectTypeEntity[] = [];
  /** control for filter for server side. */
  filterTypeEntity: FormControl = new FormControl();
  /** indicate search operation is in progress */
  searching = false;
  /** list of typeEntity filtered after simulating server side search */
  filteredTypeEntity: ReplaySubject<SelectTypeEntity[]> = new ReplaySubject<SelectTypeEntity[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  interfaceForm = this.formBuilder.group({
    interfaceId: "",
    typologyInterface: ["", [Validators.required, Validators.maxLength(10)]],
    interfaceDescription: ["", [Validators.required, Validators.maxLength(80)]],
    terminalIp: ["", [Validators.maxLength(30)]],
    terminalPort: ["", [Validators.maxLength(10)]],
    deviceId: 0,
    router: ["", [Validators.maxLength(16)]],
    coordinator: ["", [Validators.maxLength(1)]],
    inputChannelNumber: 0,
    outputChannelNumber: 0,
    interfaceGroupId: ["", [Validators.required, Validators.maxLength(10)]],
    interfaceGroupDescription: ["", [Validators.required, Validators.maxLength(100)]],
  });

  addingParam = false;
  addParamForm = this.formBuilder.group({
    entity: ["INTERFACCIA", [Validators.maxLength(50)]],
    paramName: ["", [Validators.required, Validators.maxLength(100)]],
    paramDefaultValue: "",
    type: "",
  })

  paramInterfaceForm = this.formBuilder.group({
    paramValue: ["", [Validators.required, Validators.maxLength(255)]]
  })

  paramsColumns: string[] = ['paramName', 'paramDefaultValue', 'paramValue', 'actions'];
  channelsColumns: string[] = ['direction', 'description', 'actions'];

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<InterfaceGroupsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private interfaceGroupService: InterfaceGroupService,
    private interfaceService: InterfaceService,
    private typeEntityService: typeEntityService,
    private paramService: ParamInterfaceService,
    private channelService: ChannelService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  ngOnInit(): void {
    this.getAllTypology();
    this.interfaceId = this.data.interface?.interfaceId;
    this.typologyInterface = this.data.interfaceGroup?.interfaces[0].typologyInterface;
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

    this.mode = this.data.mode;
    if (this.data.interface != null) {
      this.getNChannel(this.data.interface.typologyInterface);
    }
    if (this.IsEditMode()) {
      this.interfaceForm = this.formBuilder.group({
        interfaceId: this.data.interface?.interfaceId,
        typologyInterface: [this.data.interfaceGroup?.interfaces[0].typologyInterface, [Validators.required, Validators.maxLength(10)]],
        interfaceDescription: [this.data.interface?.interfaceDescription, [Validators.required, Validators.maxLength(80)]],
        terminalIp: [this.data.interface?.terminalIp, [Validators.required, Validators.maxLength(30)]],
        terminalPort: [this.data.interface?.terminalPort, [Validators.required, Validators.maxLength(10)]],
        deviceId: this.data.interface?.deviceId,
        router: [this.data.interface?.router, [Validators.maxLength(16)]],
        coordinator: [this.data.interface?.coordinator, [Validators.maxLength(1)]],
        inputChannelNumber: this.data.interface?.inputChannelNumber,
        outputChannelNumber: this.data.interface?.outputChannelNumber,
        interfaceGroupId: [this.data.interfaceGroup?.id, [Validators.required, Validators.maxLength(10)]],
        interfaceGroupDescription: [this.data.interfaceGroup?.description, [Validators.required, Validators.maxLength(100)]],
      });
      this.interfaceForm.get('typologyInterface')?.disable();
    }
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
    const dialogRef = this.dialog.open(TypeEntityComponent, {
      width: '80%',
      maxWidth: '90%',
      data: { mode: Mode.New, entity: "INTERFACCIA" }
    });
    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); this.interfaceForm.get('typologyInterface')?.setValue(result.id); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); this.interfaceForm.get('typologyInterface')?.setValue(result.id); });
  }

  removeTypology() {
    this.interfaceForm.get('typologyInterface')?.setValue("");
  }

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
    this.interfaceGroup = {
      id:  this.interfaceForm.get('interfaceGroupId')?.value,
      description: this.interfaceForm.get('interfaceGroupDescription')?.value,
      gatewayId: this.data.gatewayId,
      interfaces: []
    }

    if (this.IsNewMode()) {
      this.subscribers.push(this.interfaceGroupService.Add(this.interfaceGroup).subscribe(
        res => {
          this.subscribers.push(this.interfaceService.add(this.interfaceForm.value).subscribe(
            res => {
              this.closeDialog();
              this.snackBar.open(res.message, "Done", {
                duration: 2000,
                panelClass: "success"
              });
            },
            err => {
              this.subscribers.push(this.interfaceGroupService.Delete(this.interfaceForm.get('interfaceGroupId')?.value).subscribe())
            }
          ));
        }
      ));
    }
    else if (this.IsEditMode()) {
      if (this.interfaceForm.get('interfaceGroupId')?.value != this.data.interfaceGroup?.id || this.interfaceForm.get('interfaceGroupDescription')?.value != this.data.interfaceGroup?.description)
      this.subscribers.push(this.interfaceGroupService.Edit(this.interfaceGroup).subscribe(
        res => {
          this.closeDialog();
          this.snackBar.open(res.message, "Done", {
            duration: 2000,
            panelClass: "success"
          });
        }
      ));
      this.subscribers.push(this.interfaceService.edit(this.interfaceForm.value).subscribe(
        res => {
          this.closeDialog();
          this.snackBar.open(res.message, "Done", {
            duration: 2000,
            panelClass: "success"
          });
        }
      ))
    }
  }

  deleteInterface() {
    this.subscribers.push(this.interfaceService.delete(this.data.interface?.interfaceId).subscribe(
      res => {
        this.closeDialog();
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

  closeDialog() {
    this.dialogRef.close(null);
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
