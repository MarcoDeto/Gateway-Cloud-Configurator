import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GatewayDetailsComponent } from './gateway-detail.component';

describe('GatewayComponent', () => {
  let component: GatewayDetailsComponent;
  let fixture: ComponentFixture<GatewayDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GatewayDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GatewayDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
