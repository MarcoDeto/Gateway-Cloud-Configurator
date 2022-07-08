import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeEntityComponent } from './type-entity.component';

describe('TypeEntityComponent', () => {
  let component: TypeEntityComponent;
  let fixture: ComponentFixture<TypeEntityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TypeEntityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TypeEntityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
