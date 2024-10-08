// const plugin = require('tailwindcss/plugin')
const colors = require("tailwindcss/colors");
module.exports = {
    content: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],
    darkMode: "class", // or 'media' or 'class'
    theme: {
        extend: {
            width: {
                '128': '32rem',
            },
            colors: {
                primary: "var(--primary)",
                secondary: "var(--secondary)",
                main: "var(--main)",
                background: "var(--background)",
                header: "var(--header)",
                accent: "var(--accent)"
            },
            borderRadius: {
                tiny: "3px",
                base: "4px",
                md: ".125rem",
                lg: ".25rem",
                xl: ".5rem",
                "2xl": "1rem",
                "3xl": "1.5rem",
                "4xl": "2rem",
                "5xl": "3rem",
                "6xl": "4rem",
                "7xl": "5rem"
            },
            animation: {
                //   'spin-slow': 'spin 3s linear infinite',
                //   wiggle: 'wiggle 1s ease-in-out infinite'
            },
            transitionDuration: {
                0: "0ms",
                2000: "2000ms",
                3000: "3000ms"
            }
        },

        screens: {
            sm: "480px",
            md: "768px",
            lg: "976px",
            xl: "1440px"
        },
        fontSize: {
            tiny: ".5rem",
            base: ".75rem",
            sm: ".875rem",
            md: "1rem",
            lg: "1.125rem",
            xl: "1.25rem",
            "2xl": "1.5rem",
            "3xl": "1.875rem",
            "4xl": "2.25rem",
            "5xl": "3rem",
            "6xl": "4rem",
            "7xl": "5rem"
        },
        fontWeight: {
            "extra-light": 100,
            thin: 200,
            light: 300,
            normal: 400,
            medium: 500,
            semibold: 600,
            bold: 700,
            "extra-bold": 800,
            black: 900
        },
        padding: {
            tiny: ".25rem",
            base: ".5rem",
            lg: "1.125rem",
            xl: "1.25rem",
            "2xl": "1.5rem",
            "3xl": "1.875rem",
            "4xl": "2.25rem",
            "5xl": "3rem",
            "6xl": "4rem",
            "7xl": "5rem"
        },
        // https://www.tailwindshades.com/#color=42B983&step-up=10&step-down=10&name=ocean&overrides=e30%3D
        colors: {
            ...colors,
            current: "currentColor",
            white: "#ffffff",
            purple: "#3f3cbb",
            midnight: "#121063",
            metal: "#565584",
            tahiti: {
                100: "#cffafe",
                200: "#a5f3fc",
                300: "#67e8f9",
                400: "#22d3ee",
                500: "#06b6d4",
                600: "#0891b2",
                700: "#0e7490",
                800: "#155e75",
                900: "#164e63"
            },
            silver: "#ecebff",
            "bubble-gum": "#ff77e9",
            bermuda: "#78dcca",
            purple: {
                100: "#faf5ff",
                200: "#e9d8fd",
                300: "#d6bcfa",
                400: "#b794f4",
                500: "#9f7aea",
                600: "#805ad5",
                700: "#6b46c1",
                800: "#553c9a",
                900: "#44337a"
            },
            eunry: {
                DEFAULT: "#CBA3A3",
                50: "#FFFFFF",
                100: "#FFFFFF",
                200: "#FFFFFF",
                300: "#F0E4E4",
                400: "#DDC4C4",
                500: "#CBA3A3",
                600: "#B98282",
                700: "#A66262",
                800: "#884D4D",
                900: "#673A3A"
            },
            "medium-purple": {
                DEFAULT: "#9F7AEA",
                50: "#FFFFFF",
                100: "#FFFFFF",
                200: "#FEFEFF",
                300: "#DFD2F8",
                400: "#BFA6F1",
                500: "#9F7AEA",
                600: "#7F4EE3",
                700: "#6023DB",
                800: "#4C1CAF",
                900: "#391583"
            },
            pink: {
                100: "#fff5f7",
                200: "#fed7e2",
                300: "#fbb6ce",
                400: "#f687b3",
                500: "#ed64a6",
                600: "#d53f8c",
                700: "#b83280",
                800: "#97266d",
                900: "#702459"
            },
            ocean: {
                DEFAULT: "#42B983",
                50: "#E9F7F1",
                100: "#D6F1E5",
                200: "#B1E3CC",
                300: "#8BD6B4",
                400: "#66C89C",
                500: "#42B983",
                600: "#359368",
                700: "#276E4E",
                800: "#1A4833",
                900: "#0C2319"
            },
            orange: {
                50: "#fff7ed",
                100: "#ffedd5",
                200: "#fed7aa",
                300: "#fdba74",
                400: "#fb923c",
                500: "#f97316",
                600: "#ea580c",
                700: "#c2410c",
                800: "#9a3412",
                900: "#7c2d12"
            },
            brown: {
                50: "#fdf8f6",
                100: "#f2e8e5",
                200: "#eaddd7",
                300: "#e0cec7",
                400: "#d2bab0",
                500: "#bfa094",
                600: "#a18072",
                700: "#977669",
                800: "#846358",
                900: "#43302b"
            },
            slate: {
                50: "#F8FAFC",
                100: "#F1F5F9",
                200: "#E2E8F0",
                300: "#CBD5E1",
                400: "#94A3B8",
                500: "#64748B",
                600: "#475569",
                700: "#334155",
                800: "#1E293B",
                900: "#0F172A"
            },

            arctic: {
                100: "#eef5fc",
                200: "#ddebf8",
                300: "#cce0f5",
                400: "#bbd6f1",
                500: "#aaccee",
                600: "#88a3be",
                700: "#667a8f",
                800: "#44525f",
                900: "#222930"
            },
            regal: {
                100: "#d3d8de",
                200: "#a7b1bd",
                300: "#7c8a9c",
                400: "#50637b",
                500: "#243c5a",
                600: "#1d3048",
                700: "#162436",
                800: "#0e1824",
                900: "#070c12"
            }
        },
        fontFamily: {
            sans: ["Graphik", "sans-serif"],
            serif: ["Merriweather", "serif"]
        },
        spacing: {
            px: "1px",
            0: "0",
            0.5: "0.125rem",
            1: "0.25rem",
            1.5: "0.375rem",
            2: "0.5rem",
            2.5: "0.625rem",
            3: "0.75rem",
            3.5: "0.875rem",
            4: "1rem",
            5: "1.25rem",
            6: "1.5rem",
            7: "1.75rem",
            8: "2rem",
            9: "2.25rem",
            10: "2.5rem",
            11: "2.75rem",
            12: "3rem",
            14: "3.5rem",
            16: "4rem",
            20: "5rem",
            24: "6rem",
            28: "7rem",
            32: "8rem",
            36: "9rem",
            40: "10rem",
            44: "11rem",
            48: "12rem",
            52: "13rem",
            56: "14rem",
            60: "15rem",
            64: "16rem",
            72: "18rem",
            80: "20rem",
            96: "24rem",
            128: "32rem",
            144: "36rem"
        },
        boxShadow: {
            sm: "0 1px 2px 0 rgba(0, 0, 0, 0.05)",
            DEFAULT:
                "0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)",
            md:
                "0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)",
            lg:
                "0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)",
            xl:
                "0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)",
            "2xl": "0 25px 50px -12px rgba(0, 0, 0, 0.25)",
            "3xl": "0 35px 60px -15px rgba(0, 0, 0, 0.3)",
            inner: "inset 0 2px 4px 0 rgba(0, 0, 0, 0.06)",
            none: "none"
        }
    },
    variants: {
        extend: {
            rotate: ["active", "group-hover"],
            borderColor: ["focus-visible"],
            opacity: ["disabled"]
        }
    },
    // daisyui: {
    //     themes: [
    //         {
    //             lcars: {
    //
    //                 "primary": "#9c698a",
    //
    //                 "secondary": "#EB9870",
    //
    //                 "accent": "#EC943A",
    //
    //                 "neutral": "#272c35",
    //
    //                 "base-100": "#8b72aa",
    //
    //                 "info": "#81abd7",
    //
    //                 "success": "#4ddb96",
    //
    //                 "warning": "#fdb35e",
    //
    //                 "error": "#e77b73",
    //
    //             },
    //         },
    //     ],
    // },

    plugins: [require("daisyui"), require("@tailwindcss/typography")],

    content: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"]
};
