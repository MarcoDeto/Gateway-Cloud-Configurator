import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Channel } from 'src/app/Core/Models/Channel.model';
import { Mode } from 'src/app/Core/Models/Mode.model';
import { VirtualChannelService } from 'src/app/Core/Services/virtualChannel.service';
import Swal from 'sweetalert2';
import { VirtualChannelDatailsComponent } from './virtual-channel-datails/virtual-channel-datails.component';
import { typeEntityService } from 'src/app/Core/Services/typeEntity.service';
import { TypeEntity } from '../../../Models/Entity.model';
import { TypeEntityComponent } from '../../type-entity/type-entity.component';

@Component({
  selector: 'virtual-channels',
  templateUrl: './virtual-channels.component.html',
  styleUrls: ['./virtual-channels.component.scss']
})
export class VirtualChannelsComponent implements OnInit {
  @Input()
  interfaceId: string | null = "";

  entity_CANALE = "CANALE"; entity_REGOLA = "REGOLA"
  direction_INPUT = "INPUT"; direction_OUTPUT = "OUTPUT";

  subscribers: Subscription[];
  virtualChannels: Channel[] = [];
  channelsColumns: string[] = ['channelId', 'channelType', 'direction', 'description', 'ruleId', 'actions'];
  channelTypes: TypeEntity[] = []; rules: TypeEntity[] = [];

  addingChannel = false; editingChannel = false;

  virtualChannelForm = this.formBuilder.group({
    interfaceId: ["", [Validators.required, Validators.maxLength(3)]],
    channelId: ["", [Validators.maxLength(2)]],
    channelType: ["", [Validators.required, Validators.maxLength(10)]],
    description: ["", [Validators.required, Validators.maxLength(255)]],
    direction: ["", [Validators.required, Validators.maxLength(6)]],
    flgVirtual: true,
    ruleId: ["", [Validators.maxLength(10)]],
    destination: ["", [Validators.maxLength(10)]],
    originChannelId: ["", [Validators.maxLength(10)]]
  })

  constructor(
    private formBuilder: FormBuilder,
    private service: VirtualChannelService,
    private entityService: typeEntityService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.getChannelTypes();
    this.getRules();
    this.getvirtualChannels();
    this.virtualChannelForm.get('interfaceId')?.setValue(this.interfaceId);
  }

  getChannelTypes() {
    this.subscribers.push(this.entityService.getByEntity(this.entity_CANALE).subscribe(
      res => { this.channelTypes = res; }
    ))
  }

  getRules() {
    this.subscribers.push(this.entityService.getByEntity(this.entity_REGOLA).subscribe(
      res => { this.rules = res; }
    ));
  }

  addRule() {
    const dialogRef = this.dialog.open(TypeEntityComponent, {
      width: '80%',
      maxWidth: '90%',
      data: { mode: Mode.New, entity: this.entity_REGOLA }
    });
    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); this.virtualChannelForm.get('ruleId')?.setValue(result.id); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); this.virtualChannelForm.get('ruleId')?.setValue(result.id); });
  }

  getvirtualChannels() {
    this.editingChannel = false;
    this.addingChannel = false;
    this.virtualChannels = [];
    this.subscribers.push(this.service.GetVirtualChannelsByInterfaceId(this.interfaceId).subscribe(
      res => { this.virtualChannels = res; }
    ));
  }

  showDetail(element: Channel) {
    const dialogRef = this.dialog.open(VirtualChannelDatailsComponent, {
      width: '90%',
      maxWidth: '90%',
      data: { channel: element }
    });
    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  new() {
    this.virtualChannels = [];
    this.addingChannel = true;
  }

  editRow(element: Channel) {
    this.virtualChannels = [];
    this.editingChannel = true;
    this.virtualChannelForm.get('channelId')?.setValue(element.channelId);
    this.virtualChannelForm.get('channelType')?.setValue(element.channelType);
    this.virtualChannelForm.get('description')?.setValue(element.description);
    this.virtualChannelForm.get('ruleId')?.setValue(element.ruleId);
  }

  addVirtualChannel() {
    this.subscribers.push(this.service.Post(this.virtualChannelForm.value).subscribe(
      res => {
        this.virtualChannels = [];
        this.subscribers.push(this.service.GetVirtualChannelsByInterfaceId(this.interfaceId).subscribe(
          end => {
            this.addingChannel = false;
            this.virtualChannels = end;
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
          }
        ));
      }
    ));
  }

  editVirtualChannel() {
    this.subscribers.push(this.service.Put(this.virtualChannelForm.value).subscribe(
      res => {
        this.virtualChannels = [];
        this.subscribers.push(this.service.GetVirtualChannelsByInterfaceId(this.interfaceId).subscribe(
          end => {
            this.editingChannel = false;
            this.virtualChannels = end;
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
          }
        ));
      }
    ));
  }

  disableSubmit() {
    if (this.virtualChannelForm.get('channelType')?.valid && this.virtualChannelForm.get('description')?.valid && this.virtualChannelForm.get('ruleId')?.valid)
      return false;
    else
      return true;
  }

  submitForm() {
    let channelType = this.virtualChannelForm.get('channelType')?.value;
    if (channelType == 'XDO' || channelType == 'XOO') { this.virtualChannelForm.get('direction')?.setValue(this.direction_OUTPUT); }
    else if (channelType == 'XDI' || channelType == 'XOI') { this.virtualChannelForm.get('direction')?.setValue(this.direction_INPUT); }

    if (this.addingChannel) { this.addVirtualChannel() }
    else if (this.editingChannel) { this.editVirtualChannel(); }
  }

  cancel() {
    this.virtualChannels = [];
    this.subscribers.push(this.service.GetVirtualChannelsByInterfaceId(this.interfaceId).subscribe(
      res => { this.virtualChannels = res; }
    ));
    this.addingChannel = false;
    this.editingChannel = false;
  }

  DeleteVirtualChannel(element: Channel) {
    Swal.fire({
      title: 'Sicuro di voler eliminare questo canale?',
      icon: 'question',
      showDenyButton: true,
      showCancelButton: true,
      showConfirmButton: false,
      denyButtonText: "Elimina",
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isDenied) {
        this.subscribers.push(this.service.Delete(element).subscribe(
          res => {
            this.subscribers.push(this.service.GetVirtualChannelsByInterfaceId(this.interfaceId).subscribe(
              end => {
                this.virtualChannels = end;
                this.snackBar.open(res.message, "Done", {
                  duration: 2000,
                  panelClass: "success"
                });
              }
            ))
          }
        ));
      }
    });
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
