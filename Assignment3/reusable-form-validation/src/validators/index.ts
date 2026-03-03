export type ValidatorFn = (value: string) => string | null;

export function required(): ValidatorFn {
  return (value: string) => {
    if (value.trim().length === 0) {
      return "This field is required";
    }
    return null;
  };
}

export function minLength(min: number): ValidatorFn {
  return (value: string) => {
    if (value.length > 0 && value.length < min) {
      return `Minimum length is ${min}`;
    }
    return null;
  };
}

export function isEmail(): ValidatorFn {
  return (value: string) => {
    if (value.length > 0 && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      return "Invalid email format";
    }
    return null;
  };
}
