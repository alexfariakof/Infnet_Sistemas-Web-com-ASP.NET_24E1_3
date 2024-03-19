import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ToolBarComponent } from './tool-bar.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { AuthProvider } from 'src/app/provider/auth.provider';

describe('ToolBarComponent', () => {
  let component: ToolBarComponent;
  let fixture: ComponentFixture<ToolBarComponent>;
  let authProvider: AuthProvider;
  let router: Router;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ToolBarComponent],
      imports: [HttpClientTestingModule]
    });
    fixture = TestBed.createComponent(ToolBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    authProvider = TestBed.inject(AuthProvider);
    router = TestBed.inject(Router);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('isAuthenticated should return true when user is not authenticated', () => {
    // Arrange
    spyOn(authProvider, 'isAuthenticated').and.returnValue(false);

    // Act
    const result = component.isAuthenticated();

    // Assert
    expect(result).toBeTrue();
    expect(authProvider.isAuthenticated).toHaveBeenCalled();
  });

  it('isAuthenticated should return false when user is authenticated', () => {
    // Arrange
    spyOn(authProvider, 'isAuthenticated').and.returnValue(true);

    // Act
    const result = component.isAuthenticated();

    // Assert
    expect(result).toBeFalse();
    expect(authProvider.isAuthenticated).toHaveBeenCalled();
  });

  it('logout should call authService clearLocalStorage method and navigate to root route', () => {
    // Arrange
    spyOn(authProvider, 'clearLocalStorage');
    spyOn(router, 'navigate');

    // Act
    component.logout();

    // Assert
    expect(authProvider.clearLocalStorage).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/']);
  });
});
