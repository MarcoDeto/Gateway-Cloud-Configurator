import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChannelAssociateComponent } from './channel-associate.component';

describe('ChannelAssociateComponent', () => {
  let component: ChannelAssociateComponent;
  let fixture: ComponentFixture<ChannelAssociateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChannelAssociateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChannelAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
