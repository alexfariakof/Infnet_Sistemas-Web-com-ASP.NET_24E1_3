import { CommonModule } from '@angular/common';
import { Component,  Inject, OnInit } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import * as dayjs from 'dayjs';
import { Address, Customer, Merchant } from 'src/app/model';
import { AddressService, CustomerService, MerchantService } from 'src/app/services';

@Component({
  selector: 'app-account-form',
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, FlexLayoutModule],
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})

export default abstract class AccountComponent implements OnInit {
  createAccountForm: (FormGroup & (Customer | Merchant)) | any;
  eyeIconClass: string = 'bi-eye';
  eyeIconClassConfirmPassword: string = 'bi-eye';
  showPassword = false;
  showConfirmPassword = false;
  showCnpjField = true;
  showBirthField = true;

  constructor(
    @Inject(CustomerService) @Inject(MerchantService) protected service: CustomerService | MerchantService,
    protected formbuilder: FormBuilder,
    public router: Router,
    protected addressService: AddressService) { }

  ngOnInit(): void {
    const address: Address = {};
    this.createAccountForm = this.formbuilder.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required]],
      birth: [dayjs().format('YYYY-MM-DD'), Validators.required],
      cpf: ['', [Validators.required]],
      cnpj: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
      zipcode: ['', [Validators.required]],
      street: ['', [Validators.required]],
      number: [''],
      complement: [''],
      neighborhood: ['', [Validators.required]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      cardNumber: ['', [Validators.required]],
      cardValidate: ['', [Validators.required]],
      cardCVV: ['', [Validators.required]],
      address: [address],
    }) as (FormGroup & (Customer | Merchant)) | any;;

    const accountType = this.router.url.includes('customer') ? 'customer' : 'merchant';

    if (accountType === 'customer') {
      this.createAccountForm.get('cnpj').disable();
      this.showCnpjField = false;
    }
    else if (accountType === 'merchant') {
      this.createAccountForm.get('birth').disable();
      this.showBirthField = false;
    }


  }

  abstract onSaveClick(): void;

  buscarCEP() {
    const cep = this.createAccountForm.get('zipcode').value.replace('-', '');
    this.addressService.buscarCep(cep).subscribe((address: Address) => {
      if (address) {
        this.createAccountForm.patchValue({
          street: address.street,
          neighborhood: address.neighborhood,
          city: address.city,
          state: address.state,
          address: address
        });
      } else {
        alert('Endereço não encontrado!')
      }
    });
  }

  onTooglePassword(): void {
    this.showPassword = !this.showPassword;
    this.eyeIconClass = (this.eyeIconClass === 'bi-eye') ? 'bi-eye-slash' : 'bi-eye';
  }

  onToogleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
    this.eyeIconClassConfirmPassword = (this.eyeIconClassConfirmPassword === 'bi-eye') ? 'bi-eye-slash' : 'bi-eye';
  }

  isPasswordValid(): boolean {
    let senha = this.createAccountForm.get('password').value;
    let confirmaSenha = this.createAccountForm.get('confirmPassword').value;
    return (senha !== confirmaSenha);
  }
}
