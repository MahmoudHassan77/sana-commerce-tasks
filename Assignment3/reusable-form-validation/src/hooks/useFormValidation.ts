import { useReducer, useCallback, useMemo } from "react";
import { ValidatorFn } from "../validators";


export type ValidationConfig<T extends Record<string, string>> = {
  [K in keyof T]?: ValidatorFn[];
};


export interface UseFormValidationReturn<T extends Record<string, string>> {
  values: Record<keyof T, string>;
  errors: Record<keyof T, string>;
  isValid: boolean;
  setValue: (field: string, value: any) => void;
  validate: () => boolean;
  reset: () => void;
}

type FormState<T extends Record<string, string>> = {
  values: Record<keyof T, string>;
  errors: Record<keyof T, string>;
  validated: boolean;
};

type Action<T extends Record<string, string>> =
  | { type: "SET_VALUE"; field: string; value: any; validators?: ValidatorFn[] }
  | {
      type: "VALIDATE";
      config: ValidationConfig<T>;
      fieldKeys: (keyof T)[];
    }
  | { type: "RESET"; initial: Record<keyof T, string> };


function formReducer<T extends Record<string, string>>(
  state: FormState<T>,
  action: Action<T>
): FormState<T> {
  switch (action.type) {
    case "SET_VALUE": {
      state.values[action.field as keyof T] = action.value;
      let fieldError = "";
      if (state.validated && action.validators) {
        for (const validator of action.validators) {
          const error = validator(state.values[action.field as keyof T]);
          if (error) {
            fieldError = error;
            break;
          }
        }
      }
      state.errors[action.field as keyof T] = fieldError;

      return state;
    }
    case "VALIDATE": {
      const newErrors = {} as Record<keyof T, string>;

      for (const field of action.fieldKeys) {
        const validators = action.config[field] || [];
        const value = state.values[field];

        let fieldError = "";
        for (const validator of validators) {
          const error = validator(value);
          if (error) {
            fieldError = error;
            break;
          }
        }
        newErrors[field] = fieldError;
      }

      return { ...state, errors: newErrors, validated: true };
    }
    case "RESET":
      return {
        values: action.initial,
        errors: Object.keys(action.initial).reduce(
          (acc, key) => ({ ...acc, [key]: "" }),
          {} as Record<keyof T, string>
        ),
        validated: false,
      };
    default:
      return state;
  }
}


export function useFormValidation<T extends Record<string, string>>(
  config: ValidationConfig<T>,
  initialValues?: Partial<T>
): UseFormValidationReturn<T> {
  const fieldKeys = useMemo(
    () => Object.keys(config) as (keyof T)[],
    [config]
  );

  const defaultValues = useMemo(
    () =>
      fieldKeys.reduce(
        (acc, key) => ({
          ...acc,
          [key]: initialValues?.[key] ?? "",
        }),
        {} as Record<keyof T, string>
      ),
    [fieldKeys, initialValues]
  );

  const [state, dispatch] = useReducer(formReducer<T>, {
    values: defaultValues,
    errors: fieldKeys.reduce(
      (acc, key) => ({ ...acc, [key]: "" }),
      {} as Record<keyof T, string>
    ),
    validated: false,
  });

  const setValue = useCallback((field: string, value: any) => {
    dispatch({ type: "SET_VALUE", field, value, validators: config[field as keyof T] });
  }, [config]);

  const validate = useCallback((): boolean => {
    dispatch({ type: "VALIDATE", config, fieldKeys });

    let valid = true;
    for (const field of fieldKeys) {
      const validators = config[field] || [];
      const value = state.values[field];
      for (const validator of validators) {
        if (validator(value)) {
          valid = false;
          break;
        }
      }
      if (!valid) break;
    }
    return valid;
  }, [config, fieldKeys, state.values]);

  const reset = useCallback(() => {
    dispatch({ type: "RESET", initial: defaultValues });
  }, [defaultValues]);

  const isValid =
    state.validated &&
    fieldKeys.every((key) => state.errors[key] === "");

  return {
    values: state.values,
    errors: state.errors,
    isValid,
    setValue,
    validate,
    reset,
  };
}
