import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Address } from 'src/app/model';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  constructor(public httpClient: HttpClient) { }

  public buscarCep(cep: string): Observable<Address> {
    return this.httpClient.get(`https://viacep.com.br/ws/${cep}/json`).pipe(
      map((response: any) => {
        return {
          zipcode: response.cep,
          street: response.logradouro,
          complement: response.complemento,
          neighborhood: response.bairro,
          city: response.localidade,
          state: response.uf,
          country: 'br'
        };
      })
    );
  }
}
