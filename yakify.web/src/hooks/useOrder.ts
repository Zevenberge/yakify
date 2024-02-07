import { useNavigate } from "react-router-dom";
import { useDate } from "../contexts/DateContext";
import { PostProps, usePost } from "./usePost";

export type PlaceOrderDto = {
  customer: string;
  order: OrderDto;
};

export type OrderDto = {
  milk: number;
  skins: number;
};

export function useOrder(props: Omit<PostProps<OrderDto>, 'url'>) {
  const [date] = useDate();
  const post = usePost<PlaceOrderDto, OrderDto>({
    url: `/yak-shop/order/${date}`,
    ...props
  });
  return post;
}
