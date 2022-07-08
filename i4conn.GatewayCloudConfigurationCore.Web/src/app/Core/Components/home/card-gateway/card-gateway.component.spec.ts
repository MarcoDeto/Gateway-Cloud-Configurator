import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardGatewayComponent } from './card-gateway.component';

describe('CardGatewayComponent', () => {
  let component: CardGatewayComponent;
  let fixture: ComponentFixture<CardGatewayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardGatewayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CardGatewayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
