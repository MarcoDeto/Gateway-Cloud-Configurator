import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VirtualChannelDatailsComponent } from './virtual-channel-datails.component';

describe('VirtualChannelDatailsComponent', () => {
  let component: VirtualChannelDatailsComponent;
  let fixture: ComponentFixture<VirtualChannelDatailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VirtualChannelDatailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VirtualChannelDatailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
