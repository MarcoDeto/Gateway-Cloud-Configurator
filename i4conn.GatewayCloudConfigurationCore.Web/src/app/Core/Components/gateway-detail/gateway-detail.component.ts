import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ValidatorsService } from 'src/app/Shared/Validators/validators.services';
import { Gateway } from '../../Models/Gateway.model';
import { Mode } from '../../Models/Mode.model';
import { GatewayService } from '../../Services/gateway.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'gateway-detail',
  templateUrl: './gateway-detail.component.html',
  styleUrls: ['./gateway-detail.component.scss']
})
export class GatewayDetailsComponent implements OnInit, OnDestroy {
  title = 'Gateway'
  gatewayId = this.route.snapshot.paramMap.get('gatewayId');
  subscribers: Subscription[] = [];
  gateway: Gateway | undefined = undefined;
  mode: Mode = Mode.New;
  destSec = false;
  hover = false;
  versionSupervisors: string[] = [];
  versionDevices: string[] = [];
  versionRouters: string[] = [];
  versionRuleses: string[] = [];
  versionWebapps: string[] = [];
  defaultFirmwareRoot= "C:\\i4conn\\Server\\Firmware";

  gatewayForm = this.formBuilder.group({
    gatewayId: ["", [Validators.required, Validators.maxLength(3)]],
    gatewayName: ["", [Validators.required, Validators.maxLength(30)]],
    description: ["", [Validators.required, Validators.maxLength(255)]],
    serialNumber: ["", [Validators.required, Validators.maxLength(15)]],
    location: ["", [Validators.required, Validators.maxLength(50)]],
    destinationIp: ["", [Validators.required, Validators.maxLength(15)]],
    destinationPort: ["5672", [Validators.required, Validators.maxLength(10)]],
    destinationUser: ["I4IOTGateway", [Validators.required, Validators.maxLength(20)]],
    destinationPassword: ["I4IOTGateway", [Validators.required, Validators.maxLength(20)]],
    versionSupervisor: ["", [Validators.maxLength(255)]],
    versionDevice: ["", [Validators.maxLength(255)]],
    versionRouter: ["", [Validators.maxLength(255)]],
    versionRules: ["", [Validators.maxLength(255)]],
    firmwareRoot: [this.defaultFirmwareRoot, [Validators.required, Validators.maxLength(255)]],
    versionWebapp: ["", [Validators.maxLength(255)]],
    destinationSecondaryIp: ["", [Validators.maxLength(15)]],
    destinationSecondaryPort: ["", [Validators.maxLength(10)]],
    destinationSecondaryUser: ["", [Validators.maxLength(20)]],
    destinationSecondaryPassword: ["", [Validators.maxLength(20)]],
  });

  constructor(
    private route: ActivatedRoute,
    private gatewayService: GatewayService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private validators: ValidatorsService
  ) {
    this.subscribers.push(this.gatewayService.getFirmwares(this.defaultFirmwareRoot).subscribe(
      res => {
        this.populatesLists(res);
      }
    ));
  }

  /// REGION SELECT LIST
  findSupervisor(firmware: any) { return firmware.name === 'Supervisor'; }
  findDeviceManager(firmware: any) { return firmware.name === 'DeviceManager'; }
  findRouter(firmware: any) { return firmware.name === 'Router'; }
  findRulesManager(firmware: any) { return firmware.name === 'RulesManager'; }
  findWebApp(firmware: any) { return firmware.name === 'WebApp'; }
  populatesLists(res: any) {
    this.versionSupervisors = res.find(this.findSupervisor)?.versions;
    this.versionDevices = res.find(this.findDeviceManager)?.versions;
    this.versionRouters = res.find(this.findRouter)?.versions;
    this.versionRuleses = res.find(this.findRulesManager)?.versions;
    this.versionWebapps = res.find(this.findWebApp)?.versions;
  }
  cercaVersioni(path: string) {
    this.subscribers.push(this.gatewayService.getFirmwares(path).subscribe(
      res => {
        this.populatesLists(res);
      }
    ));
  }
  /// END REGION SELECT LIST

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  ngOnInit(): void {
    if (this.gatewayId != null && this.gatewayId != 'Add') {
      this.mode = Mode.Edit;
      this.subscribers.push(this.gatewayService.getById(this.gatewayId).subscribe(
        res => {
          this.gateway = res;
          this.gatewayForm = this.formBuilder.group({
            gatewayId: [this.gateway?.gatewayId, [Validators.required, Validators.maxLength(3)]],
            gatewayName: [this.gateway?.gatewayName, [Validators.required, Validators.maxLength(30)]],
            description: [this.gateway?.description, [Validators.required, Validators.maxLength(255)]],
            serialNumber: [this.gateway?.serialNumber, [Validators.required, Validators.maxLength(15)]],
            location: [this.gateway?.location, [Validators.required, Validators.maxLength(50)]],
            destinationIp: [this.gateway?.destinationIp, [Validators.required, Validators.maxLength(15)]],
            destinationPort: [this.gateway?.destinationPort, [Validators.required, Validators.maxLength(10)]],
            destinationUser: [this.gateway?.destinationUser, [Validators.required, Validators.maxLength(20)]],
            destinationPassword: [this.gateway?.destinationPassword, [Validators.required, Validators.maxLength(20)]],
            versionSupervisor: [this.gateway?.versionSupervisor, [Validators.maxLength(255)]],
            versionDevice: [this.gateway?.versionDevice, [Validators.maxLength(255)]],
            versionRouter: [this.gateway?.versionRouter, [Validators.maxLength(255)]],
            versionRules: [this.gateway?.versionRules, [Validators.maxLength(255)]],
            firmwareRoot: [this.gateway?.firmwareRoot, [Validators.required, Validators.maxLength(255)]],
            versionWebapp: [this.gateway?.versionWebapp, [Validators.maxLength(255)]],
            destinationSecondaryIp: [this.gateway?.destinationSecondaryIp, [Validators.maxLength(15)]],
            destinationSecondaryPort: [this.gateway?.destinationSecondaryPort, [Validators.maxLength(10)]],
            destinationSecondaryUser: [this.gateway?.destinationSecondaryUser, [Validators.maxLength(20)]],
            destinationSecondaryPassword: [this.gateway?.destinationSecondaryPassword, [Validators.maxLength(20)]],
          });
          this.findInvalidControls();
        }
      ));
    }
  }

  public findInvalidControls() {
    const invalid = [];
    const controls = this.gatewayForm.controls;
    for (const name in controls) {
        if (controls[name].invalid) {
            invalid.push(name);
        }
    }
  }

  public AddDestSec() {
    this.destSec = true;
  }

  public Dest() {
    this.destSec = false;
  }

  back() {
    if (this.gatewayId != "Add") { this.router.navigate(['gateway/interfacegroups', this.gatewayId]); }
    else { this.router.navigate(['home']); }
  }

  onSubmit() {
    if (this.IsNewMode()) {
      this.subscribers.push(this.gatewayService.Add(this.gatewayForm.value).subscribe(
        res => {
          this.snackBar.open(res.message, "Done", {
            duration: 2000,
            panelClass: "success"
          });
          this.router.navigate(['home']);
        }
      ));
    }
    else {
      this.subscribers.push(this.gatewayService.Edit(this.gatewayForm.value).subscribe(
        res => {
          this.snackBar.open(res.message, 'Done', {
            duration: 2000,
            panelClass: "success"
          });
          this.router.navigate(['home']);
        }
      ));
    }
  }

  Delete() {
    Swal.fire({
      title: 'Sicuro di voler eliminare questo gateway?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.subscribers.push(this.gatewayService.Delete(this.gatewayId).subscribe(
          res => {
            this.snackBar.open(res.message, "Done", {
              duration: 2000,
              panelClass: "success"
            });
            this.router.navigate(['home']);
          },
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
