import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { CountryContext } from "../../providers/CountryProvider";
import { UserDataContext } from "../../providers/UserDataProvider";


export default function UserEdit() {
    const [userData, setUserData] = useState({ id: '', nameFirst: '', nameLast: '', pseudonym: '', email: '', city: '', state: '', countryId: '', isActive: true });
    const [isLoading, setIsLoading] = useState(true);


    const { countries, getAllCountries } = useContext(CountryContext)
    const { updateUser, getUserById } = useContext(UserDataContext);

    const history = useHistory();

    const thisUser = JSON.parse(sessionStorage.UserData)

    const handleFieldChange = evt => {
        const stateToChange = { ...userData }
        stateToChange[evt.target.id] = evt.target.value
        setUserData(stateToChange)
    }

    const submit = (e) => {
        setIsLoading(true)
        userData.countryId = parseInt(userData.countryId);
        updateUser(userData).then((u) => {
            history.push(`/users/${thisUser.id}`);
        });
    };

    const CancelSave = (evt) => {
        history.push("/users");
    }

    useEffect(() => {
        getAllCountries();
        getUserById(thisUser.id)
            .then(setUserData)
        setIsLoading(false)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);


    if (!userData) {
        return null;
    }
    return (
        (!isLoading) ?

            <div className="container">
                <div className="row justify-content-center">
                    <div className="card col-md-12 col-lg-8">
                        <h3 className="mt-3 text-primary text-center card-title">User Data Edit</h3>
                        <div className="mt-5 card-body">
                            <div className="text-danger"></div>
                            <div className="form-group">
                                <label htmlFor="nameFirst" className="control-label">First Name</label>
                                <input
                                    id="nameFirst"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={userData.nameFirst}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="nameLast" className="control-label">Last Name</label>
                                <input
                                    id="nameLast"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={userData.nameLast}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="city" className="control-label">City</label>
                                <input
                                    id="city"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={userData.city}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="state" className="control-label">State</label>
                                <input
                                    id="state"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={userData.state}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="countryId" className="control-label">Country</label>
                                <select
                                    id="countryId"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={userData.countryId}
                                >
                                    {countries.map(aCountry =>
                                        <option value={aCountry.id} key={aCountry.id}>{aCountry.name}</option>
                                    )}
                                </select>
                            </div>

                            <div className="form-group">
                                <button disabled={isLoading} onClick={submit} className="btn btn-primary btn-block">Save Changes</button>
                                <button disabled={isLoading} onClick={CancelSave} className="btn btn-primary btn-block">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div> : null
    )
}
