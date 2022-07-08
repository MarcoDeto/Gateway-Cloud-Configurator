import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { AssociableChannel, Channel } from 'src/app/Core/Models/Channel.model';
import { ChannelAssociateService } from 'src/app/Core/Services/channelAssociate.service';
import Swal from 'sweetalert2';
import { ChannelAssociate } from '../../../../../Models/Channel.model';

@Component({
  selector: 'channel-associate',
  templateUrl: './channel-associate.component.html',
  styleUrls: ['./channel-associate.component.scss']
})
export class ChannelAssociateComponent implements OnInit {
  @Input()
  channel: Channel | undefined = undefined;

  subscribers: Subscription[] = [];
  dataSource: ChannelAssociate[] = [];
  channelsColumns: string[] = [];
  detailsColumns = ['channelId', 'chType', 'direction', 'chDescription', 'flgAbilitaOriginale', 'actions'];
  formColumns = ['chDirection', 'channel', 'flg', 'actions'];
  associableChannels: AssociableChannel[] = [];
  addingChannel = false; editingChannel = false;

  associateForm = this.formBuilder.group({});

  constructor(
    private formBuilder: FormBuilder,
    private service: ChannelAssociateService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.channelsColumns = this.detailsColumns;
    if (this.channel != undefined) {
      this.getAssociateChannels();
      this.associateForm = this.formBuilder.group({
        interfaceId: [this.channel?.interfaceId, [Validators.required, Validators.maxLength(3)]],
        virtualChannelId: [this.channel?.channelId, [Validators.required, Validators.maxLength(2)]],
        channelId: ["", [Validators.required, Validators.maxLength(2)]],
        direction: ["", [Validators.required, Validators.maxLength(6)]],
        flgAbilitaOriginale: false,
        chDescription: "",
        virtualChDescription: "",
        chType: "",
        virtualChType: ""
      });
    }
  }

  getAssociateChannels() {
    if (this.channel != undefined) {
      this.subscribers.push(this.service.getAssociateChannels(this.channel).subscribe(
        res => { this.dataSource = res; }
      ));
    }
  }

  getAssociableChannels() {
    if (this.channel != undefined) {
      let interfaceId = this.channel.interfaceId;
      let direction = this.associateForm.get('direction')?.value;
      this.subscribers.push(this.service.getAssociableChannels(interfaceId, direction).subscribe(
        res => { this.associableChannels = res; }
      ));
    }
  }

  new() {
    this.dataSource = [];
    this.addingChannel = true;
    this.channelsColumns = this.formColumns;
  }

  editRow(element: Channel) {
    this.dataSource = [];
    this.channelsColumns = this.formColumns;
    this.editingChannel = true;
    this.associateForm.get('direction')?.setValue(element.direction);
    this.getAssociableChannels();
    this.associateForm.get('channelId')?.setValue(element.channelId);
  }

  addAssociate() {
    this.subscribers.push(this.service.post(this.associateForm.value).subscribe(
      res => {
        this.dataSource = [];
        if (this.channel != undefined) {
          this.subscribers.push(this.service.getAssociateChannels(this.channel).subscribe(
            end => {
              this.dataSource = end;
              this.addingChannel = false;
              this.channelsColumns = this.detailsColumns;
              this.snackBar.open(res.message, "Done", {
                duration: 2000,
                panelClass: "success"
              });
            }
          ));
        }
      }
    ));
  }

  editAssociate() {
    this.subscribers.push(this.service.put(this.associateForm.value).subscribe(
      res => {
        this.dataSource = [];
        if (this.channel != undefined) {
          this.subscribers.push(this.service.getAssociateChannels(this.channel).subscribe(
            end => {
              this.dataSource = end;
              this.channelsColumns = this.detailsColumns;
              this.editingChannel = false;
              this.snackBar.open(res.message, "Done", {
                duration: 2000,
                panelClass: "success"
              });
            }
          ));
        }
      }
    ));
  }

  submitForm() {
    let channelType = this.associateForm.get('channelType')?.value;
    //if (channelType == 'XDO' || channelType == 'XOO') { this.associateForm.get('direction')?.setValue(this.direction_OUTPUT); }
    //else if (channelType == 'XDI' || channelType == 'XOI') { this.associateForm.get('direction')?.setValue(this.direction_INPUT); }

    if (this.addingChannel) { this.addAssociate() }
    else if (this.editingChannel) { this.editAssociate(); }
  }

  cancel() {
    this.dataSource = [];
    this.channelsColumns = this.detailsColumns;
    if (this.channel != undefined) {
      this.subscribers.push(this.service.getAssociateChannels(this.channel).subscribe(
        res => { this.dataSource = res; }
      ));
    }
    this.addingChannel = false;
    this.editingChannel = false;
  }

  DeleteAssociate(element: ChannelAssociate) {
    Swal.fire({
      title: 'Sicuro di voler eliminare questa associazione?',
      icon: 'question',
      showDenyButton: true,
      showCancelButton: true,
      showConfirmButton: false,
      denyButtonText: "Elimina",
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isDenied) {
        this.subscribers.push(this.service.delete(element).subscribe(
          res => {
            if (this.channel != undefined) {
              this.subscribers.push(this.service.getAssociateChannels(this.channel).subscribe(
                end => {
                  this.dataSource = end;
                  this.snackBar.open(res.message, "Done", {
                    duration: 2000,
                    panelClass: "success"
                  });
                }
              ));
            }
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
