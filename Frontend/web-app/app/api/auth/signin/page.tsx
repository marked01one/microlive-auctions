'use client'

import EmptyFilter from '@/app/components/EmptyFilter'
import React from 'react'

export default function Page({searchParams}: {searchParams: {callbackUrl: string}}) {
  return (
    <EmptyFilter 
      title='You need to login to do that'
      subtitle='Please click below to login'
      showLogin
      callbackUrl={searchParams.callbackUrl}
    />
  )
}
