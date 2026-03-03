
import { useFormValidation, ValidationConfig } from "../hooks/useFormValidation";
import { required, minLength, isEmail } from "../validators";
import type { FormEvent } from "react";

type FormFields = {
  name: string;
  email: string;
};

const formConfig: ValidationConfig<FormFields> = {
  name: [required(), minLength(3)],
  email: [required(), isEmail()],
};

enum Fields {
  Name = "name",
  Email = "email",
}

export function ReusableForm() {
  const { values, errors, isValid, setValue, validate, reset } =
    useFormValidation<FormFields>(formConfig);

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    const valid = validate();
    if (valid) {
      alert(`Form submitted!\nName: ${values.name}\nEmail: ${values.email}`);
    }
  };

  return (
    <div className="form-card">
      <h2 className="form-title">Contact Form</h2>
      <p className="form-subtitle">
        Reusable validation hook demo — Assignment 3
      </p>
      <div className="form-accent-bar" />

      <form onSubmit={handleSubmit}>
        <div className="field-group">
          <label htmlFor="name" className="field-label">
            Name
          </label>
          <input
            id="name"
            type="text"
            placeholder="Enter your full name"
            value={values.name}
            onChange={(e) => setValue(Fields.Name, e.target.value)}
            className={`field-input ${errors.name ? "field-input--error" : ""}`}
          />
          {errors.name && <span className="field-error">{errors.name}</span>}
        </div>

        <div className="field-group">
          <label htmlFor="email" className="field-label">
            Email
          </label>
          <input
            id="email"
            type="text"
            placeholder="you@example.com"
            value={values.email}
            onChange={(e) => setValue(Fields.Email, e.target.value)}
            className={`field-input ${errors.email ? "field-input--error" : ""}`}
          />
          {errors.email && <span className="field-error">{errors.email}</span>}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn-primary">
            Submit
          </button>
          <button type="button" onClick={reset} className="btn btn-secondary">
            Reset
          </button>
        </div>

        <div
          className={`form-status ${isValid ? "form-status--valid" : "form-status--invalid"}`}
        >
          {isValid ? "All fields are valid" : "Please fill in all fields correctly"}
        </div>
      </form>
    </div>
  );
}



