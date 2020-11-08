import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateProjectComponent } from './add-update-project.component';

describe('AddUpdateProjectComponent', () => {
  let component: AddUpdateProjectComponent;
  let fixture: ComponentFixture<AddUpdateProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUpdateProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
