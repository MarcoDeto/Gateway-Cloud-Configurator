import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Gateway } from '../../Models/Gateway.model';
import { GatewayService } from '../../Services/gateway.service';
import {TranslateService} from '@ngx-translate/core';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  title = 'Home';

  localid = navigator.language;
  hover = false;
  subscribers: Subscription[] = [];
  gateways: Gateway[] = [];

  constructor(
    private route: ActivatedRoute,
    public translate: TranslateService,
    private gatewayService: GatewayService,
  ) {

    translate.addLangs(['en', 'it']);
    translate.setDefaultLang('en');

    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/en|it/) ? browserLang : 'en');
   }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }

  ngOnInit(): void {
    this.getGateways();
  }

  getGateways() {
    this.subscribers.push(this.gatewayService.getAll().subscribe(
      res => {
        this.gateways = res;
      }
    ));
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

}
