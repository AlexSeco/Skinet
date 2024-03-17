import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Brand } from '../models/brand';
import { Product } from '../models/product';
import { ShopParams } from '../models/shopParams';
import { Type } from '../models/type';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: Product[] = []
  brands: Brand[] = []
  types: Type[] = []
  shopParams: ShopParams;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to high', value: 'priceAsc' },
    { name: 'Price: High to low', value: 'priceDesc' }
  ];
  totalCount = 0;
  constructor(private shopService: ShopService) { 
    this.shopParams = shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts().subscribe({
      next: response => {
        console.log("Cached Brands Response:", response); // Log the cached response
        this.products = response.data;

        this.totalCount = response.count;
      },
      error: error => console.log(error)
    })
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: response => {
        console.log("Cached Brands Response:", response); // Log the cached response
        this.brands = response; // Assigning the array directly
      },
      error: error => console.log(error)
    });
  }
  
  getTypes() {
    this.shopService.getTypes().subscribe({
      next: response => {
        console.log("Cached Types Response:", response); // Log the cached response
        this.types = response; // Assigning the array directly
      },
      error: error => console.log(error)
    });
  }


  onBrandSelected(brandId: number) {
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onSortSelected(event: any) {
    const params = this.shopService.getShopParams();
    params.sort = event.target.value;
    this.getProducts();
  }


  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();

    if (params.pageIndex !== event) {
      params.pageIndex = event;
      this.shopService.setShopParams(params);
      this.shopParams = params;
      this.getProducts();
    }
  }

  onSearch() {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm?.nativeElement.value;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.shopParams = this.shopParams;
    this.getProducts();
  }
}
