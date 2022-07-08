import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Channel } from 'src/app/Core/Models/Channel.model';
import { ChannelService } from 'src/app/Core/Services/channel.service';
import Swal from 'sweetalert2';
import { VariableChannel } from '../../../Models/Channel.model';

@Component({
  selector: 'channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class ChannelsComponent implements OnInit, OnDestroy {
  @Input()
  interfaceId: string | null = "";

  subscribers: Subscription[];
  channels: Channel[] = [];
  Columns: string[] = [];
  channelsColumns: string[] = ['channelId', 'channelType', 'direction', 'description', 'variables', 'actions'];
  formColumns: string[] = ['channelId', 'channelType', 'direction', 'description'];
  digitalChannelsColumns: string[] = ['channelId', 'channelType', 'direction', 'description', 'nameVar', 'keyVar', 'actions'];
  expandedElement: VariableChannel | null = null;

  allowEditChannel = false;
  editingChannel = false;

  channelForm = this.formBuilder.group({
    description: ["", [Validators.required, Validators.maxLength(255)]]
  })

  constructor(
    private formBuilder: FormBuilder,
    private channelService: ChannelService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.getChannels();
    this.Columns = this.channelsColumns;

  }

  expandTable(element: any) {
    if (this.editingChannel == false) { this.expandedElement = this.expandedElement === element ? null : element}
  }

  getChannels() {
    this.editingChannel = false;
    this.channels = [];
    this.subscribers.push(this.channelService.getAllByInterfaceId(this.interfaceId).subscribe(
      res => {
        console.log(res);
        this.channels = res;
        if (res.length > 0) { this.allowEditChannel = res[0].allowEditChannel; }
        }
    ));
  }

  editRowChannel(element: Channel) {
    this.channels = [];
    this.channels.push(element);
    this.editingChannel = true;
    this.Columns = this.formColumns;
    this.channelForm.get('description')?.setValue(element.description);
  }

  cancel() {
    this.editingChannel = false;
    this.Columns = [];
    this.Columns = this.channelsColumns;
    this.subscribers.push(this.channelService.getAllByInterfaceId(this.interfaceId).subscribe(
      res => { this.channels = res; }
    ));
  }

  editChannel(element: Channel) {
    element.description = this.channelForm.get('description')?.value;
    this.subscribers.push(this.channelService.put(element).subscribe(
      res => {
        this.editingChannel = false;
        this.Columns = this.channelsColumns;
        this.subscribers.push(this.channelService.getAllByInterfaceId(this.interfaceId).subscribe(
          end => {
            this.channels = end;
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
          }
        ));
      }
    ));
  }

  DeleteChannel(element: Channel) {
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
        this.subscribers.push(this.channelService.delete(element).subscribe(
          res => {
            this.subscribers.push(this.channelService.getAllByInterfaceId(this.interfaceId).subscribe(
              end => {
                this.channels = end;
                this.snackBar.open(res.message, "Done", {
                  duration: 2000,
                  panelClass: "success"
                });
              }
            ));
          }
        ));
      }
    });
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
