import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Gateway } from 'src/app/Core/Models/Gateway.model';

@Component({
  selector: 'card-gateway',
  templateUrl: './card-gateway.component.html',
  styleUrls: ['./card-gateway.component.scss']
})
export class CardGatewayComponent implements OnInit, OnDestroy {
  @Input()
  gateway: Gateway | undefined = undefined;
  hover = false;

  constructor() { }

  ngOnInit(): void {
  }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }

  ngOnDestroy(): void {
  }

}
