import { Component, Input } from '@angular/core';
import { Product } from '../../models/product';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product?: Product;

  constructor(private basketService: BasketService) { }

  addItemToBasket() {
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
