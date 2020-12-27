import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateTaskComponent } from './add-update-task.component';

describe('AddUpdateTaskComponent', () => {
  let component: AddUpdateTaskComponent;
  let fixture: ComponentFixture<AddUpdateTaskComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUpdateTaskComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
