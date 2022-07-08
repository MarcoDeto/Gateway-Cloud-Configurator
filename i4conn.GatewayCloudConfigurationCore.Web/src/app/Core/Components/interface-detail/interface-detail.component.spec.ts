import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterfaceDetailComponent } from './interface-detail2.component';

describe('InterfaceDetailComponent', () => {
  let component: InterfaceDetailComponent;
  let fixture: ComponentFixture<InterfaceDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InterfaceDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InterfaceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
