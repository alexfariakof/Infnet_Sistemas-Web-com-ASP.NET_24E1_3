import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { CustomerComponent } from './customer.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { CustomerService, AddressService } from 'src/app/services';
import { Address, Customer } from 'src/app/model';
import { of } from 'rxjs';

describe('CustomerComponent', () => {
  let component: CustomerComponent;
  let fixture: ComponentFixture<CustomerComponent>;
  let customerService: CustomerService;
  let addressService: AddressService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, BrowserAnimationsModule, RouterTestingModule],
      providers:[AddressService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerComponent);
    component = fixture.componentInstance;
    customerService = TestBed.inject(CustomerService);
    addressService = TestBed.inject(AddressService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('onSaveClick should call customer service Create method', fakeAsync(() => {
    // Arrange
    const mockAddress: Address = {
      zipcode: '99999-999',
      street: 'Rua Example',
      complement: '',
      neighborhood: 'Bairro Example',
      city: 'Cidade Example',
      state: 'Estado Example',
      country: 'BR'
    };
    const mockCustomer: Customer = {
      name: 'Teste',
      email: 'teste@teste.com',
      password: 'teste',
      cpf: '999.999.999-99',
      birth: '2024-01-01',
      phone: '(99) 9999-9999',
      address: mockAddress,
      flatId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      card: {
        number: '',
        validate: '',
        cvv: ''
      }
    };
    spyOn(customerService, 'create').and.returnValue(of(mockCustomer));
    spyOn(component, 'onSaveClick').and.callThrough();
    spyOn(component.router, 'navigate');


    // Act
    component.createAccountForm.patchValue(mockCustomer);
    component.onSaveClick();
    tick();

    // Assert
    expect(customerService.create).toHaveBeenCalled();
    expect(customerService.create).toHaveBeenCalledWith(mockCustomer);
    expect(component.router.navigate).toHaveBeenCalledWith(['/login']);
  }));

});
