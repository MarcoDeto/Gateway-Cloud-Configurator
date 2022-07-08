import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Channel, VariableChannel } from 'src/app/Core/Models/Channel.model';
import { ChannelVariablesService } from 'src/app/Core/Services/channelVariables.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'variables',
  templateUrl: './variables.component.html',
  styleUrls: ['./variables.component.scss']
})
export class VariablesComponent implements OnInit, OnDestroy {
  @Input()
  channel: Channel | undefined = undefined;

  subscribers: Subscription[] = [];
  dataSource: VariableChannel[] = [];
  channelsColumns: string[] = ['name', 'key', 'group', 'actions'];

  allowAllName = false;
  addingVariable = false;
  editingVariable = false;

  VarForm = this.formBuilder.group({});

  constructor(
    private formBuilder: FormBuilder,
    private service: ChannelVariablesService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    if (this.channel?.variables != undefined) {
      this.dataSource = this.channel?.variables;
      this.VarForm = this.formBuilder.group({
        interfaceId: [this.channel?.interfaceId, [Validators.required, Validators.maxLength(3)]],
        channelId: [this.channel?.channelId, [Validators.required, Validators.maxLength(2)]],
        direction: [this.channel?.direction, [Validators.required, Validators.maxLength(6)]],
        name: ["", [Validators.required, Validators.maxLength(30)]],
        group: ["", [Validators.required, Validators.maxLength(30)]],
        key: ["", [Validators.required, Validators.maxLength(500)]]
      });
    }
  }

  getVariables() {
    if (this.channel != undefined) {
      this.subscribers.push(this.service.GetVariable(this.channel).subscribe(
        res => {
          this.dataSource = res;
        }
      ));
    }
  }

  new() {
    this.addingVariable = true;
    this.dataSource = [];
  }

  addVariable() {
    this.subscribers.push(this.service.PostVariable(this.VarForm.value).subscribe(
      res => {
        this.addingVariable = false;
        this.dataSource = [];
        if (this.channel != undefined) {
          this.subscribers.push(this.service.GetVariable(this.channel).subscribe(
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

  cancel() {
    this.dataSource = [];
    if (this.channel?.variables != undefined) {
      this.subscribers.push(this.service.GetVariable(this.channel).subscribe(
        res => {
          this.dataSource = res;
        }
      ));
      this.addingVariable = false;
      this.editingVariable = false;
    }
  }

  editRow(element: VariableChannel) {
    this.dataSource = [];
    this.editingVariable = true;
    this.VarForm.get('group')?.setValue(element.group);
    this.VarForm.get('name')?.setValue(element.name);
    this.VarForm.get('key')?.setValue(element.key);
    if (this.channel?.channelType == "XDI") { this.allowAllName = false; }
    else { this.allowAllName = true; }
  }

  editVariable() {
    this.subscribers.push(this.service.PutVariable(this.VarForm.value).subscribe(
      res => {
        this.editingVariable = false;
        if (this.channel != undefined) {
          this.subscribers.push(this.service.GetVariable(this.channel).subscribe(
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

  submitForm() {
    if (this.addingVariable) { this.addVariable(); }
    else if (this.editingVariable) { this.editVariable(); }
  }

  DeleteVariable(element: VariableChannel) {
    Swal.fire({
      title: 'Sicuro di voler eliminare questa variabile?',
      icon: 'question',
      showDenyButton: true,
      showCancelButton: true,
      showConfirmButton: false,
      denyButtonText: "Elimina",
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isDenied) {
        this.subscribers.push(this.service.DeleteVariable(element).subscribe(
          res => {
            if (this.channel != undefined) {
              this.subscribers.push(this.service.GetVariable(this.channel).subscribe(
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

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

}
