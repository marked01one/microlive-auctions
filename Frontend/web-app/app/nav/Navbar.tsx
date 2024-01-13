'use client'

import React from 'react';
import {AiOutlineCar} from 'react-icons/ai';
import Search from './Search';
import { useParamsStore } from '@/hooks/useParamsStore';

export default function Navbar() {
  const reset = useParamsStore(state => state.reset);
  
  return (
    <header className='sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md'>
      <div onClick={reset} className='flex items-center gap-2 text-3xl font-semibold text-red-500 cursor-pointer'>
        <AiOutlineCar size={34} />
        <div>Auction App</div>
      </div>
      
      <Search />
      <div>Login</div>
    </header>
  )
}
