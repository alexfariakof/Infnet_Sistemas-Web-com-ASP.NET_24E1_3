import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Customer } from 'src/app/model';
import { AddressService, CustomerService } from 'src/app/services';
import AccountComponent from '../account.component';

@Component({
  selector: 'app-customer',
  templateUrl: './../account.component.html',
  styleUrls: ['./../account.component.css']
})

export class CustomerComponent extends AccountComponent {

  constructor(
    public cutomerService: CustomerService,
    formbuilder: FormBuilder,
    router: Router,
    addressService: AddressService
  ) {
    super(cutomerService, formbuilder, router, addressService);
  }

  onSaveClick = (): void => {
    let customer: Customer | any = {
      flatId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      name: this.createAccountForm.get("name").value,
      email: this.createAccountForm.get("email").value,
      password: this.createAccountForm.get("password").value,
      cpf: this.createAccountForm.get("cpf").value,
      birth: this.createAccountForm.get("birth").value,
      phone: this.createAccountForm.get("phone").value,
      address: this.createAccountForm.get("address").value,
      card: {
        number: this.createAccountForm.get("cardNumber").value,
        validate: this.createAccountForm.get("cardValidate").value,
        cvv: this.createAccountForm.get("cardCVV").value
      }
    };
    customer.address.number = this.createAccountForm.get("number").value;
    customer.address.complement = this.createAccountForm.get("complement").value;
    customer.address.neighborhood = this.createAccountForm.get("neighborhood").value;
    customer.address.city = this.createAccountForm.get("city").value;
    customer.address.state = this.createAccountForm.get("state").value;

    this.service.create(customer)
    .subscribe({
      next: (response:any) => {
        if (response != null) {
          this.router.navigate(['/login']);
        }
        else {
          throw (response);
        }
      },
      error :(response : any) =>  {
        alert(response.error);
      },
      complete() {

      }
    });
  }
}
