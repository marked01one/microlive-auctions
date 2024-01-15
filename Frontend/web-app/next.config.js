/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        remotePatterns: [
            {
                protocol: "https",
                hostname: "cdn.pixabay.com",
                port: '',
                pathname: '/photo/**'
            }
        ]
    },
    logging: {
        fetches: {
            fullUrl: true
        }
    }
}

module.exports = nextConfig
