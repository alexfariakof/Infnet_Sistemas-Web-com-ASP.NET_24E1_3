import { TestBed } from '@angular/core/testing';
import { LoadingComponent } from './loading.component';

describe('Unit Test LoadingComponent', () => {
  let component: LoadingComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [LoadingComponent]
    });
    component = new LoadingComponent();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });
});
