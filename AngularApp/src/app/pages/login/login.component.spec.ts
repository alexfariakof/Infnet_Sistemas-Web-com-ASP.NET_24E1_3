import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from 'src/app/services';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: AuthService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, BrowserAnimationsModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule],
      providers:[AuthService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('onLoginClick should call authService signIn method and navigate to myplaylist on success', fakeAsync(() => {
    // Arrange
    spyOn(authService, 'signIn').and.returnValue(of({ authenticated: true }));
    spyOn(component.router, 'navigate');

    // Act
    component.onLoginClick();
    tick();

    // Assert
    expect(authService.signIn).toHaveBeenCalled();
    expect(component.router.navigate).toHaveBeenCalledWith(['/']);
  }));

  it('onLoginClick should display error alert on authentication failure', fakeAsync(() => {
    // Arrange
    const errorResponse = { error: 'Authentication failed' };
    spyOn(authService, 'signIn').and.returnValue(throwError(errorResponse));
    spyOn(window, 'alert');

    // Act
    component.onLoginClick();
    tick();

    // Assert
    expect(authService.signIn).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(errorResponse.error);
  }));

  it('onTooglePassword should toggle showPassword property and change eye icon class', () => {
    // Arrange
    const initialShowPasswordValue = component.showPassword;
    const initialEyeIconClassValue = component.eyeIconClass;

    // Act
    component.onTooglePassword();

    // Assert
    expect(component.showPassword).toBe(!initialShowPasswordValue);
    expect(component.eyeIconClass).not.toBe(initialEyeIconClassValue);
  });
});
