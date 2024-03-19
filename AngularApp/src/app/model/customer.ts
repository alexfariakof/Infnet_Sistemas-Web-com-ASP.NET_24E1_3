import { Dayjs } from "dayjs";
import { Address } from "./address";
import { Card } from "./card";

export interface Customer {
  name: string;
  email: string;
  password: string;
  cpf: string;
  birth: Dayjs | string | null;
  phone: string;
  address: Address;
  flatId: string;
  card: Card;
}
