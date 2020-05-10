-------------控件定义--------------
-- !@#start

--!@#definestart
LoginItem = 
{
    --Image
    Img_Bg = nil;
    --Button
    Btn_Login = nil;
    --Button
    Btn_Quit = nil;

}
--!@#defineend

--!@#get/setstart
function LoginItem:GetImage(image, sprite)
    if image ~= nil then
        return image.sprite;
    end
end
function LoginItem:SetImage(image, sprite)
    if image ~= nil then
        image.sprite = sprite;
    end
end

--!@#get/setend

-------------输入监听--------------
function LoginItem:InitBtnListener()
    self.csharp:AddSelfClick(self.Btn_Login.gameObject, self.OnBtn_Login, self);
    self.csharp:AddSelfClick(self.Btn_Quit.gameObject, self.OnBtn_Quit, self);

end

-- !@#startClick
function LoginItem:OnBtn_Login(go)

end
function LoginItem:OnBtn_Quit(go)

end

-- !@#endClick

function LoginItem:InitFEventListener()

end

-- !@#startFEvent

-- !@#endFEvent

-- !@#end

------------生命周期------------------
function LoginItem:Init()
	self:InitBtnListener();
	self:OnSpawn();
end

function LoginItem:Dispose()
  	self:OnRelease();
end

function LoginItem:OnSpawn()
	self:InitFEventListener();
end

function LoginItem:OnRelease()
  
end
