import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { Address } from '../models/user';


@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  constructor(private fb: FormBuilder, private accountService: AccountService) {}


  ngOnInit(): void {
    this.getAddressFormValue();
  }

  checkoutForm = this.fb.group({
    addressForm: this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zipcode: ['', Validators.required],
    }),

    deliveryForm: this.fb.group({
      deliveryMethod: ['', Validators.required]
    }),

    paymentform: this.fb.group({
      nameOnCard: ['', Validators.required]
    })
  })

  getAddressFormValue() {
    this.accountService.getUserAddress().subscribe({
      next: address => {
       this.checkoutForm.get('addressForm')?.patchValue(address)
      }
    })
  }
}
