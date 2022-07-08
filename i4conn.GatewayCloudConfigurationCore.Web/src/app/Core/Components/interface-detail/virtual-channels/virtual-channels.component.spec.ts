import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VirtualChannelsComponent } from './virtual-channels.component';

describe('VirtualChannelsComponent', () => {
  let component: VirtualChannelsComponent;
  let fixture: ComponentFixture<VirtualChannelsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VirtualChannelsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VirtualChannelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
