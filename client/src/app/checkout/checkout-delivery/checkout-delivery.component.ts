import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DeliveryMethods } from 'src/app/models/deliveryMethods';
import { CheckoutService } from '../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {

  @Input() checkoutForm?: FormGroup;
  deliveryMethods: DeliveryMethods[] = [];


  constructor(private checkoutService: CheckoutService, private basketService: BasketService) {}


  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().subscribe({
      next: dm => this.deliveryMethods = dm
    });
  }

  setShippingPrice(deliveryMethod: DeliveryMethods)
  {
    this.basketService.setShippingPrice(deliveryMethod)
  }
}
