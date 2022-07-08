import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterfaceGroupDatailComponent } from './interface-group-datail.component';

describe('InterfaceDatailComponent', () => {
  let component: InterfaceGroupDatailComponent;
  let fixture: ComponentFixture<InterfaceGroupDatailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InterfaceGroupDatailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InterfaceGroupDatailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
