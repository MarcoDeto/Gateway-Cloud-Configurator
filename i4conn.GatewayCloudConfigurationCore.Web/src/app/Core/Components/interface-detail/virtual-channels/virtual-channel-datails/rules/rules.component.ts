import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { AssociableChannel, Channel } from 'src/app/Core/Models/Channel.model';
import { ChannelAssociateService } from 'src/app/Core/Services/channelAssociate.service';
import Swal from 'sweetalert2';
import { ChannelAssociate } from '../../../../../Models/Channel.model';
import { RuleService } from '../../../../../Services/rule.service';
import { EntityParam } from '../../../../../Models/ParamInterface.model';
import { RuleRequest } from '../../../../../Models/RuleRequest.model';
import { ParamInterfaceService } from '../../../../../Services/paramInterface.service';

@Component({
  selector: 'rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.scss']
})
export class RulesComponent implements OnInit {
  @Input()
  channel: Channel | undefined = undefined;

  subscribers: Subscription[] = [];
  dataSource: EntityParam[] = [];
  channelsColumns: string[] = ['paramName', 'paramValue', 'actions'];
  associableChannels: AssociableChannel[] = [];
  addingRule = false; editingRule = false;

  ruleForm = this.formBuilder.group({});

  ruleRequest: RuleRequest | undefined = undefined;
  request: EntityParam | undefined = undefined;

  constructor(
    private formBuilder: FormBuilder,
    private service: RuleService,
    private paramService: ParamInterfaceService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    if (this.channel != undefined) {
      this.getRule();
      this.ruleForm = this.formBuilder.group({
        paramName: ["", [Validators.required, Validators.maxLength(100)]],
        paramValue: ["", [Validators.required, Validators.maxLength(255)]]
      });
    }
  }

  getRule() {
    if (this.channel != undefined) {
      this.subscribers.push(this.service.GetRuleParams(this.channel).subscribe(
        res => { this.dataSource = res; }
      ));
    }
  }

  new() {
    this.dataSource = [];
    this.addingRule = true;
    this.editingRule = false;
  }

  editRow(element: Channel) {
    this.dataSource = [];
    this.addingRule = false;
    this.editingRule = true;
  }

  addRule(request: RuleRequest) {
    this.subscribers.push(this.service.PostRule(request).subscribe(
      res => {
        this.dataSource = [];
        if (this.channel != undefined) {
          this.subscribers.push(this.service.GetRuleParams(this.channel).subscribe(
            end => {
              this.dataSource = end;
              this.addingRule = false;
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

  editRule(request: RuleRequest) {
    this.subscribers.push(this.service.PutRule(request).subscribe(
      res => {
        this.dataSource = [];
        if (this.channel != undefined) {
          this.subscribers.push(this.service.GetRuleParams(this.channel).subscribe(
            end => {
              this.dataSource = end;
              this.editingRule = false;
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
    this.request = {
      entityId: "", paramDefaultValue: "", type: "", useDefault: false,
      entity: "REGOLA",
      paramName: this.ruleForm.get("paramName")?.value,
      paramValue: this.ruleForm.get("paramValue")?.value
    }
    if (this.channel != undefined && this.request != undefined) {
      this.ruleRequest = {
        direction: this.channel?.direction,
        interfaceId: this.channel.interfaceId,
        virtualCh: this.channel.channelId,
        content: this.request
      }
    }
    if (this.ruleRequest != undefined) {
      if (this.addingRule) { this.addRule(this.ruleRequest); }
      else if (this.editingRule) { this.editRule(this.ruleRequest); }
    }
  }

  cancel() {
    this.dataSource = [];
    if (this.channel != undefined) {
      this.subscribers.push(this.service.GetRuleParams(this.channel).subscribe(
        res => { this.dataSource = res; }
      ));
    }
    this.addingRule = false;
    this.editingRule = false;
  }

  DeleteRule(element: EntityParam) {
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
        this.subscribers.push(this.paramService.Default(element).subscribe(
          res => {
            if (this.channel != undefined) {
              this.subscribers.push(this.service.GetRuleParams(this.channel).subscribe(
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
