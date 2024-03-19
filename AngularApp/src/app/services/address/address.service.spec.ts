import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AddressService } from './address.service';
import { Address } from 'src/app/model';

describe('AddressService', () => {
  let service: AddressService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AddressService]
    });

    service = TestBed.inject(AddressService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch address data from API via GET', () => {
    // Arrange
    const mockResponse: any = {
      cep: '12345-678',
      logradouro: 'Rua Example',
      complemento: 'Complemento Example',
      localidade: 'Cidade Example',
      bairro: 'Bairro Example',
      uf: 'UF Example'
    };

    const mockAddress: Address = {
      zipcode: '12345-678',
      street: 'Rua Example',
      complement: 'Complemento Example',
      neighborhood: 'Bairro Example',
      city: 'Cidade Example',
      state: 'UF Example',
      country: 'br'
    };

    // Act
    service.buscarCep('12345-678').subscribe((address: Address) => {
      // Assert
      expect(address).toEqual(mockAddress);
    });

    // Assert
    const req = httpMock.expectOne(`https://viacep.com.br/ws/12345-678/json`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
    httpMock.verify();
  });
});
