import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Merchant } from 'src/app/model';
import { MerchantService, AddressService } from 'src/app/services';
import AccountComponent from '../account.component';

@Component({
  selector: 'app-merchant',
  templateUrl: './../account.component.html',
  styleUrls: ['./../account.component.css']
})
export class MerchantComponent extends AccountComponent {
  constructor(
    public merchantService: MerchantService,
    formbuilder: FormBuilder,
    router: Router,
    addressService: AddressService

  ) {
    super(merchantService, formbuilder, router, addressService);
  }

  onSaveClick = (): void => {
    let merchant: Merchant | any = {
      flatId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      name: this.createAccountForm.get("name").value,
      email: this.createAccountForm.get("email").value,
      password: this.createAccountForm.get("password").value,
      cpf: this.createAccountForm.get("cpf").value,
      cnpj: this.createAccountForm.get("cnpj").value,
      phone: this.createAccountForm.get("phone").value,
      address: this.createAccountForm.get("address").value,
      card: {
        number: this.createAccountForm.get("cardNumber").value,
        validate: this.createAccountForm.get("cardValidate").value,
        cvv: this.createAccountForm.get("cardCVV").value
      }
    };
    merchant.address.number = this.createAccountForm.get("number").value;
    merchant.address.complement = this.createAccountForm.get("complement").value;
    merchant.address.neighborhood = this.createAccountForm.get("neighborhood").value;
    merchant.address.city = this.createAccountForm.get("city").value;
    merchant.address.state = this.createAccountForm.get("state").value;

    this.merchantService.create(merchant)
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
