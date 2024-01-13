'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { Auction, PagedResult } from '@/types';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';



export default function Listings() {
  const [data, setData] = useState<PagedResult<Auction>>();
  const params = useParamsStore(state => ({
    pageNumber: state.pageNumber,
    pageCount: state.pageCount,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy
  }));
  const setParams = useParamsStore(state => state.setParams);
  const url = qs.stringifyUrl({url: '', query: params})

  function setPageNumber(pageNumber: number) {
    setParams({pageNumber})
  }


  // const [auctions, setAuctions] = useState<Auction[]>([]);
  // const [pageCount, setPageCount] = useState(0);
  // const [pageNumber, setPageNumber] = useState(1);
  // const [pageSize, setPageSize] = useState(4);

  // Create a side effect for the component
  // Execute at least once when component first load
  // Cause the component to re-render depending on what we have inside
  useEffect(() => {
    getData(url).then(data => {
      setData(data);
    });
  }, [url])  // useEffect() will be called again when `pageNumber` changes
  
  if (!data) return <h3>Loading...</h3>;

  return (
    <>
      <Filters />
      {data.totalCount === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className='grid grid-cols-4 gap-6'>
            {data.results.map(auction => (
              <AuctionCard auction={auction} key={auction.id} />
            ))}
          </div>
          <div className='flex justify-center mt-4'>
            <AppPagination 
              currentPage={params.pageNumber} pageCount={data.pageCount} 
              pageChanged={setPageNumber} />
          </div>
        </>
      )}
      
    </>
  )
}
