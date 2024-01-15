'use server'

import { Auction, PagedResult } from "../../types";
import { getTokenWorkaround } from "./authActions";

export async function getData(queryString: string): Promise<PagedResult<Auction>> {
    const result = await fetch(`http://localhost:6001/search${queryString}`);
    console.log(`http://localhost:6001/search${queryString}`);

    if (!result.ok) throw Error("Failed to fetch data!");
    console.log(result.status.toString());

    return result.json();
}


export async function updateAuctionTest() {
  const data = {
    mileage: Math.floor(Math.random() * 100000) + 1
  }

  const token = await getTokenWorkaround();

  console.log(token?.access_token)

  const res = await fetch(`http://localhost:6001/auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c`, {
    method: 'PUT',
    headers: {
      'Content-type': 'application/json',
      'Authorization': "Bearer " + token?.access_token
    },
    body: JSON.stringify(data)
  })

  if (!res.ok) return {status: res.status, msg: res.statusText}

  return res.statusText;
}