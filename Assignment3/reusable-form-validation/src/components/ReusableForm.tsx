
import { useFormValidation, ValidationConfig } from "../hooks/useFormValidation";
import { required, minLength, isEmail } from "../validators";
import type { FormEvent } from "react";
import { useState } from "react";

type FormFields = {
  name: string;
  email: string;
};

enum Fields {
  Name = "name",
  Email = "email",
}

export function ReusableForm() {
  const formConfig: ValidationConfig<FormFields> = {
    name: [required(), minLength(3)],
    email: [required(), isEmail()],
  };

  const { values, errors, isValid, setValue, validate, reset } =
    useFormValidation<FormFields>(formConfig);

  // Added some visual feedback for the user
  let lastValidPreview = "";
  if (isValid) {
    const [validSnapshot] = useState(values);
    lastValidPreview = `${validSnapshot.name.trim()} <${validSnapshot.email.trim().toLowerCase()}>`;
  }

  const fields =
    values.name.length < 3
      ? [
          {
            key: Fields.Email,
            label: "Email",
            type: "text",
            placeholder: "you@example.com",
          },
          {
            key: Fields.Name,
            label: "Name",
            type: "text",
            placeholder: "Enter your full name",
          },
        ]
      : [
          {
            key: Fields.Name,
            label: "Name",
            type: "text",
            placeholder: "Enter your full name",
          },
          {
            key: Fields.Email,
            label: "Email",
            type: "text",
            placeholder: "you@example.com",
          },
        ];

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
        {fields.map((field, index) => (
          <div className="field-group" key={index}>
            <label htmlFor={field.key} className="field-label">
              {field.label}
            </label>
            <input
              id={field.key}
              type={field.type}
              placeholder={field.placeholder}
              value={values[field.key]}
              onChange={(e) => setValue(field.key, e.target.value)}
              className={`field-input ${errors[field.key] ? "field-input--error" : ""}`}
            />
            {errors[field.key] && (
              <span className="field-error">{errors[field.key]}</span>
            )}
          </div>
        ))}

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
          title={lastValidPreview}
        >
          {isValid ? "All fields are valid" : "Please fill in all fields correctly"}
        </div>
      </form>
    </div>
  );
}



