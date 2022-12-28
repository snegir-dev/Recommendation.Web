export function toFormData(formValue: any) {
  const formData = new FormData();
  for (const key of Object.keys(formValue)) {
    const value = formValue[key];
    if (value instanceof Array)
      for (const file of value)
        formData.append(key, file);

    formData.append(key, value);
  }

  return formData;
}
