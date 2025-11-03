import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { QueryProvider } from "@/lib/providers/query-provider";
import { ThemeProvider } from "@/lib/providers/theme-provider";
import { NextIntlClientProvider } from "next-intl";
import { getMessages } from "next-intl/server";

const inter = Inter({
  subsets: ["latin"],
  display: "swap",
  variable: "--font-inter",
});

export const metadata: Metadata = {
  title: {
    default: "TeknoHaber - Türkiye'nin Teknoloji Gazetesi",
    template: "%s | TeknoHaber",
  },
  description:
    "Son dakika teknoloji haberleri, güncel gelişmeler, yapay zeka, yazılım, donanım ve teknoloji dünyasından tüm haberler. TeknoHaber ile teknolojinin nabzını tutun!",
  generator: "Next.js",
  keywords: [
    "teknohaber",
    "teknoloji haberleri",
    "güncel haberler",
    "yapay zeka",
    "yazılım haberleri",
    "teknoloji",
    "haberler",
    "Türkiye",
    "siber güvenlik",
    "donanım",
    "mobil",
    "bilim",
    "inovasyon",
  ],
  authors: [{ name: "TeknoHaber Editörleri", url: "https://teknohaber.netlify.app" }],
  creator: "TeknoHaber",
  publisher: "TeknoHaber",
  category: "technology",
  classification: "Technology News",
  formatDetection: {
    email: false,
    address: false,
    telephone: false,
  },
  metadataBase: new URL("https://teknohaber.netlify.app"),
  alternates: {
    canonical: "/",
    languages: {
      "tr-TR": "/",
    },
  },
  openGraph: {
    type: "website",
    locale: "tr_TR",
    url: "https://teknohaber.netlify.app",
    siteName: "TeknoHaber",
    title: "TeknoHaber - Türkiye'nin Teknoloji Gazetesi",
    description:
      "Son dakika teknoloji haberleri, güncel gelişmeler, yapay zeka, yazılım, donanım ve teknoloji dünyasından tüm haberler. TeknoHaber ile teknolojinin nabzını tutun!",
    images: [
      {
        url: "https://teknohaber.netlify.app/og-image.png",
        width: 1200,
        height: 630,
        alt: "TeknoHaber - Türkiye'nin Teknoloji Gazetesi",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    site: "@teknohaber_tr",
    creator: "@teknohaber_tr",
    title: "TeknoHaber - Türkiye'nin Teknoloji Gazetesi",
    description:
      "Son dakika teknoloji haberleri, güncel gelişmeler, yapay zeka, yazılım, donanım ve teknoloji dünyasından tüm haberler.",
    images: ["https://teknohaber.netlify.app/og-image.png"],
  },
  robots: {
    index: true,
    follow: true,
    nocache: false,
    googleBot: {
      index: true,
      follow: true,
      noimageindex: false,
      "max-video-preview": -1,
      "max-image-preview": "large",
      "max-snippet": -1,
    },
  },
  verification: {
    google: "google-site-verification-code",
    yandex: "yandex-verification-code",
    other: {
      "msvalidate.01": "bing-verification-code",
    },
  },
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const messages = await getMessages();
  const siteUrl = "https://teknohaber.netlify.app";

  // JSON-LD structured data for the website
  const websiteSchema = {
    "@context": "https://schema.org",
    "@type": "WebSite",
    name: "TeknoHaber",
    alternateName: "Türkiye'nin Teknoloji Gazetesi",
    url: siteUrl,
    description:
      "Son dakika teknoloji haberleri, güncel gelişmeler, yapay zeka, yazılım, donanım ve teknoloji dünyasından tüm haberler. TeknoHaber ile teknolojinin nabzını tutun!",
    inLanguage: "tr-TR",
    potentialAction: {
      "@type": "SearchAction",
      target: {
        "@type": "EntryPoint",
        urlTemplate: `${siteUrl}/search?q={search_term_string}`,
      },
      "query-input": "required name=search_term_string",
    },
    publisher: {
      "@type": "Organization",
      name: "TeknoHaber",
      url: siteUrl,
      logo: {
        "@type": "ImageObject",
        url: `${siteUrl}/og-image.png`,
        width: 1200,
        height: 630,
      },
    },
  };

  const organizationSchema = {
    "@context": "https://schema.org",
    "@type": "Organization",
    name: "TeknoHaber",
    url: siteUrl,
    logo: `${siteUrl}/og-image.png`,
    description:
      "Türkiye'nin öncü teknoloji haber platformu. Yapay zeka, yazılım, donanım, siber güvenlik ve teknoloji dünyasından güncel haberler.",
    sameAs: [
      "https://twitter.com/teknohaber_tr",
      "https://www.facebook.com/teknohaber",
      "https://www.linkedin.com/company/teknohaber",
    ],
    contactPoint: {
      "@type": "ContactPoint",
      contactType: "Customer Service",
      availableLanguage: ["Turkish"],
    },
  };

  return (
    <html lang="tr" className={inter.variable} suppressHydrationWarning>
      <head>
        <script
          type="application/ld+json"
          dangerouslySetInnerHTML={{ __html: JSON.stringify(websiteSchema) }}
        />
        <script
          type="application/ld+json"
          dangerouslySetInnerHTML={{ __html: JSON.stringify(organizationSchema) }}
        />
      </head>
      <body className="bg-background min-h-screen antialiased">
        <ThemeProvider
          attribute="class"
          defaultTheme="system"
          enableSystem
          disableTransitionOnChange
        >
          <QueryProvider>
            <NextIntlClientProvider messages={messages}>{children}</NextIntlClientProvider>
          </QueryProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}
