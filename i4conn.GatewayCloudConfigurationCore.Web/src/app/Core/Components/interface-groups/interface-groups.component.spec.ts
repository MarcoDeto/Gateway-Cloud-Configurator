import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterfaceGroupsComponent } from './interface-groups.component';

describe('InterfaceGroupsComponent', () => {
  let component: InterfaceGroupsComponent;
  let fixture: ComponentFixture<InterfaceGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InterfaceGroupsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InterfaceGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
