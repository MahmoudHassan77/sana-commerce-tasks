# Reusable Form Validation

A React + TypeScript demo showcasing a reusable form validation hook (`useFormValidation`).

Built with **React 19**, **TypeScript**, and **Vite**.

## Preview

![App Preview](Screenshot%202026-03-04%20at%201.32.33%E2%80%AFAM.png)

## Project Structure

```
src/
├── hooks/
│   └── useFormValidation.ts   # Custom hook with useReducer-based form state
├── validators/
│   └── index.ts               # Composable validator functions (required, minLength, isEmail)
├── components/
│   └── ReusableForm.tsx        # Contact form component using the hook
├── App.tsx
└── main.tsx
```

## Getting Started

```bash
# Install dependencies
npm install

# Start dev server
npm run dev

# Build for production
npm run build
```

## How It Works

1. **Define validators** — small pure functions that return an error string or `null`:
   - `required()` — ensures the field is not empty
   - `minLength(n)` — enforces a minimum character count
   - `isEmail()` — validates email format

2. **Configure the hook** — pass a validation config mapping each field to its validators:

   ```ts
   const { values, errors, isValid, setValue, validate, reset } =
     useFormValidation<ContactForm>({
       name: [required(), minLength(3)],
       email: [required(), isEmail()],
     });
   ```

3. **Use in any form** — the hook returns values, errors, and helpers that work with any set of fields.