﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace SA45TEAM07_VEHICLE
{
    public class RentVehicleControl
    {
        private MainControl mainControl;
        private FormAvailableVehicles formCategorySearch;
        private FormRentDetails formRentDetails;
        
        public MainControl MainControl
        {
            get
            {
                return mainControl;
            }

            set
            {
                mainControl = value;
            }
        }

        public FormAvailableVehicles FormCategorySearch
        {
            get
            {
                return formCategorySearch;
            }

            set
            {
                formCategorySearch = value;
            }
        }

        public FormRentDetails FormRentDetails
        {
            get
            {
                return formRentDetails;
            }

            set
            {
                formRentDetails = value;
            }
        }

        public Customer RetrieveCustomer(string NRIC)
        {
           
            RentalDAO rentalDAO = RentalDAO.Instance;

            try
            {
                rentalDAO.OpenConnection();
                return rentalDAO.RetrieveCustomer(NRIC);
            }
            catch (Exception)
            {
                throw;           // preserve stack trace     
            }
            finally
            {
                rentalDAO.CloseConnection();
            }
        }

        public RentVehicleControl(MainControl mainControl)
        {
            this.MainControl = mainControl;
            this.FormCategorySearch = new FormAvailableVehicles(this);
            FormCategorySearch.displayAvailableVehiclesUI();
            List<string> vehicleCategory = VehicleDAO.Instance.RetrieveCategoryList();
            FormCategorySearch.displayCategory(vehicleCategory);
        }

        public void UpdateVehicleStatus(Vehicle rentedVehicle)
        {
            try
            {
                VehicleDAO.Instance.OpenConnection();
                VehicleDAO.Instance.UpdateVehicleStatus(rentedVehicle);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                VehicleDAO.Instance.CloseConnection();
            }
        }

        public void CreateRentalRecord(RentalRecord record)
        {
            RentalDAO rentalDAO = RentalDAO.Instance;
            try
            {
                rentalDAO.OpenConnection();
                rentalDAO.InsertRentalRecord(record);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                rentalDAO.CloseConnection();
            }

        }

        public void SelectCategory(string category)
        {
            try
            {
                VehicleDAO.Instance.OpenConnection();
                DataTable dt = new DataTable();
                switch (category)
                {
                    case "Car":
                        dt = VehicleDAO.Instance.RetrieveAvailableCarList();
                        break;
                    case "Bus":
                        dt = VehicleDAO.Instance.RetrieveAvailableBusList();
                        break;
                    case "Truck":
                        dt = VehicleDAO.Instance.RetrieveAvailableTruckList();
                        break;
                    default:
                        break;
                }
                FormCategorySearch.displayAvailableVehicle(dt);
            }
            catch (VehicleException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                VehicleDAO.Instance.CloseConnection();
            }
        }

        public void SelectVehicle(string plateNum)
        {
            FormRentDetails = new FormRentDetails(this);
            FormRentDetails.DisplayRentalDetails(plateNum);

            try
            {
                VehicleDAO.Instance.OpenConnection();
                formRentDetails.Record.RentedVehicle = VehicleDAO.Instance.RetrieveVehicle(plateNum);
            }
            catch (Exception)
            {
                throw;           // preserve stack trace     
            }
            finally
            {
                VehicleDAO.Instance.CloseConnection();
            }
        }

        public void Close(BaseForm form)
        {
            form.Close();

            //change focus
            if (form is FormRentDetails)
            {
                this.formCategorySearch.Activate();
            }
            
        }

        public void CloseUseCase()
        {
            mainControl.CloseUseCase(this);
        }

        public void Destroy()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
