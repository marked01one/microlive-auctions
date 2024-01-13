'use server'

import { Auction, PagedResult } from "../../types";

export async function getData(queryString: string): Promise<PagedResult<Auction>> {
    const result = await fetch(`
      http://localhost:6001/search${queryString}
    `);
    console.log(`http://localhost:6001/search${queryString}`);

    if (!result.ok) throw Error("Failed to fetch data!");
    console.log(result.status.toString());

    return result.json();
}