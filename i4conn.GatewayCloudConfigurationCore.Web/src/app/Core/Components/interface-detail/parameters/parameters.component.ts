import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { EntityParam } from 'src/app/Core/Models/ParamInterface.model';
import { ParamInterfaceService } from 'src/app/Core/Services/paramInterface.service';
import Swal from 'sweetalert2';
import { Interface } from '../../../Models/Interface.model';

@Component({
  selector: 'parameters',
  templateUrl: './parameters.component.html',
  styleUrls: ['./parameters.component.scss']
})
export class ParametersComponent implements OnInit, OnDestroy {
  @Input()
  interface: Interface | undefined = undefined;

  subscribers: Subscription[];
  parameters: EntityParam[] = [];

  paramsColumns: string[] = [];

  addingParam = false;
  editingParam = false;

  addParamForm = this.formBuilder.group({
    entity: ["INTERFACCIA", [Validators.maxLength(50)]],
    paramName: ["", [Validators.required, Validators.maxLength(100)]],
    paramDefaultValue: "",
    type: "",
  })

  editParamForm = this.formBuilder.group({
    paramValue: ["", [Validators.required, Validators.maxLength(255)]]
  })

  constructor(
    private formBuilder: FormBuilder,
    private paramService: ParamInterfaceService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.getParamInterface();
    this.addParamForm.get('type')?.setValue(this.interface?.typologyInterface);
  }

  getParamInterface() {
    this.paramsColumns = ['paramName', 'paramDefaultValue', 'paramValue', 'actions'];
    this.addingParam = false;
    this.editingParam = false;
    this.parameters = [];
    this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
      res => {
        this.parameters = res;
      }
    ));
  }

  addParamInterface() {
    this.subscribers.push(this.paramService.Add(this.addParamForm.value).subscribe(
      res => {
        this.addingParam = false;
        this.parameters = [];
        this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
          res => {
            this.parameters = res;
          }
        ));
        this.snackBar.open(res.message, "Done", {
          duration: 2000,
          panelClass: "success"
        });
      }
    ));
  }

  cancel() {
    this.parameters = [];
    this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
      res => {
        this.parameters = res;
      }
    ));
    this.addingParam = false;
  }

  editRowParam(element: EntityParam) {
    this.paramsColumns = ['paramName', 'paramValue', 'actions'];
    this.editingParam = true;
    this.parameters = [];
    this.parameters.push(element);
    this.editParamForm.get('paramValue')?.setValue(element.paramValue);
  }

  editParamInterface(element: EntityParam) {
    element.paramValue = this.editParamForm.get('paramValue')?.value;
    this.subscribers.push(this.paramService.Edit(element).subscribe(
      res => {
        this.parameters = [];
        this.editingParam = false;
        this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
          res => {
            this.parameters = res;
          }
        ));
        this.snackBar.open(res.message, "Done", {
          duration: 2000,
          panelClass: "success"
        });
      }
    ))
  }

  DeleteParam(element: EntityParam) {
    Swal.fire({
      title: 'Sicuro di voler eliminare questo parametro?',
      icon: 'question',
      showDenyButton: true,
      showCancelButton: true,
      showConfirmButton: false,
      denyButtonText: "Elimina",
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isDenied) {
        if (element.useDefault) {
          this.subscribers.push(this.paramService.Delete(element).subscribe(
            res => {
              this.parameters = [];
              this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
                res => {
                  this.parameters = res;
                }
              ));
              this.snackBar.open(res.message, "Done", {
                duration: 2000,
                panelClass: "success"
              });
            }
          ));
        }
        else {
          element.useDefault = true;
          element.paramValue = '';
          this.subscribers.push(this.paramService.Default(element).subscribe(
            res => {
              this.parameters = [];
              this.subscribers.push(this.paramService.getInterfaceParams(this.interface?.interfaceId).subscribe(
                res => {
                  this.parameters = res;
                }
              ));
              this.snackBar.open(res.message, "Done", {
                duration: 2000,
                panelClass: "success"
              });
            }
          ));
        }
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
