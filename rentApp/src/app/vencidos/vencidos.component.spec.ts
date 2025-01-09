import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VencidosComponent } from './vencidos.component';

describe('VencidosComponent', () => {
  let component: VencidosComponent;
  let fixture: ComponentFixture<VencidosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [VencidosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VencidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
